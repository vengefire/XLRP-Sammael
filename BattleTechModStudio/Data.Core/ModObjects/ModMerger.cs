using System;
using System.Collections.Generic;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;
using Newtonsoft.Json.Linq;

namespace Data.Core.ModObjects
{
    public class ModMerger
    {
        public static List<(ManifestEntry, string)> FailedMerges { get; set; } = new List<(ManifestEntry, string)>();

        public static (List<ManifestEntry> mergedManifestEntries, Dictionary<Tuple<string, GameObjectTypeEnum, string>, List<ManifestEntry>> manifestEntryStackById) Merge(Manifest manifest, ModCollection modCollection)
        {
            var manifestEntryStackById = ModMerger.BuildMergeStack(manifest, modCollection);
            var mergedManifestEntries = ModMerger.ProcessMergeStack(manifestEntryStackById);
            mergedManifestEntries.Sort((entry, manifestEntry) => entry.GameObjectType.CompareTo(manifestEntry.GameObjectType));
            mergedManifestEntries.Sort((entry, manifestEntry) => entry.Id.CompareTo(manifestEntry.Id));
            return (mergedManifestEntries, manifestEntryStackById);
        }

        private static List<ManifestEntry> ProcessMergeStack(Dictionary<Tuple<string, GameObjectTypeEnum, string>, List<ManifestEntry>> manifestEntryStackById)
        {
            var mergedManifestEntries = new List<ManifestEntry>(manifestEntryStackById.Keys.Count);
            foreach (var manifestEntryStackItem in manifestEntryStackById)
            {
                ManifestEntry mergedResult = null;

                if (manifestEntryStackItem.Value.Count == 1)
                {
                    var entry = manifestEntryStackItem.Value.First();
                    if (entry.ManifestEntryGroup == null || entry.ManifestEntryGroup.AddToDb)
                    {
                        mergedResult = entry;
                    }
                }
                else
                {
                    // Console.WriteLine($"Merging {manifestEntryStackItem.Key}...");
                    var stackOrder = manifestEntryStackItem.Value;

                    if (stackOrder.Count > 1 && stackOrder.Any(entry => entry.GameObjectType == GameObjectTypeEnum.AdvancedJSONMerge))
                    {
                        var i = 666;
                    }

                    stackOrder.Sort((entry, manifestEntry) => entry.GameObjectType == manifestEntry.GameObjectType ? 0 : entry.GameObjectType == GameObjectTypeEnum.AdvancedJSONMerge ? 1 : -1);

                    if (manifestEntryStackItem.Key.Item1 == "mechdef_victor_VTR-9S")
                    {
                        var i = 777;
                    }

                    foreach (var currentManifestEntry in stackOrder)
                    {
                        if (mergedResult == null)
                        {
                            if (currentManifestEntry.ManifestEntryGroup == null || currentManifestEntry.ManifestEntryGroup.AddToDb)
                            {
                                mergedResult = currentManifestEntry;
                            }

                            continue;
                        }

                        if (currentManifestEntry.ManifestEntryGroup == null)
                        {
                            mergedResult = currentManifestEntry;
                            continue;
                        }

                        if (currentManifestEntry.ManifestEntryGroup.ShouldAppendText)
                        {
                            var targetText = mergedResult.Text;
                            var appendText = currentManifestEntry.Text;
                            mergedResult.Text = targetText += appendText;
                            continue;
                        }

                        if (!currentManifestEntry.ManifestEntryGroup.ShouldMergeJson && currentManifestEntry.GameObjectType != GameObjectTypeEnum.AdvancedJSONMerge)
                        {
                            Console.WriteLine($"Warning - Overwriting merged result for {mergedResult.Id} with mod instruction from {currentManifestEntry.ManifestEntryGroup.BaseDirectory.FullName}.");
                            mergedResult = currentManifestEntry;
                            continue;
                        }

                        if (currentManifestEntry.GameObjectType != GameObjectTypeEnum.AdvancedJSONMerge)
                        {
                            var targetJson = mergedResult.Json;
                            var mergeJson = currentManifestEntry.Json;
                            targetJson.Merge(mergeJson, new JsonMergeSettings {MergeArrayHandling = MergeArrayHandling.Union});
                            mergedResult.Json = targetJson;
                            continue;
                        }

                        // AdvancedJsonMerge...
                        var mergeSpec = currentManifestEntry.Json;
                        ((JArray) mergeSpec["Instructions"]).ToList().ForEach(instruction =>
                        {
                            var path = instruction["JSONPath"].ToString();
                            // path = path.Replace("$.", mergedResult.Id);
                            path = path.Replace("$.", string.Empty);
                            var action = instruction["Action"].ToString();
                            dynamic value = null;
                            switch (action)
                            {
                                case "Replace":
                                    var valueToReplaceWith = instruction["Value"];
                                    var valueToReplace = mergedResult.Json.SelectToken(path);

                                    if (valueToReplace == null)
                                    {
                                        ModMerger.FailedMerges.Add((currentManifestEntry, $"Failed to find value by path to replace. Path - [{path}]"));
                                        break;
                                    }

                                    if (valueToReplace.Type == JTokenType.Null)
                                    {
                                        var parent = (JProperty) valueToReplace.Parent;
                                        parent.Value = valueToReplaceWith;
                                    }
                                    else
                                    {
                                        valueToReplace.Replace(valueToReplaceWith);
                                    }

                                    break;
                                case "ArrayAdd":
                                    var valueToAdd = instruction["Value"];
                                    var targetAddArray = (JArray) mergedResult.Json.SelectToken(path);
                                    targetAddArray?.Add(valueToAdd);
                                    break;
                                case "ArrayConcat":
                                    var valuesToConcat = (JArray) instruction["Value"];
                                    var targetConcatArray = (JArray) mergedResult.Json.SelectToken(path);
                                    targetConcatArray?.Merge(valuesToConcat, new JsonMergeSettings
                                    {
                                        MergeArrayHandling = MergeArrayHandling.Union
                                    });
                                    break;
                                case "Remove":
                                    var targetToken = mergedResult.Json.SelectToken(path, false);
                                    targetToken?.Parent.Remove();
                                    break;
                            }
                        });

                        // Free the content once we've completed processing the merge directives...
                        currentManifestEntry.ClearContent();
                    }
                }

                if (mergedResult != null)
                {
                    mergedManifestEntries.Add(mergedResult);
                }
            }

            return mergedManifestEntries;
        }

        private static Dictionary<Tuple<string, GameObjectTypeEnum, string>, List<ManifestEntry>> BuildMergeStack(Manifest manifest, ModCollection modCollection)
        {
            var manifestEntryStackById = new Dictionary<Tuple<string, GameObjectTypeEnum, string>, List<ManifestEntry>>();

            modCollection.ValidModsLoadOrder.ForEach(mod =>
            {
                mod.ManifestEntries().ToList().ForEach(entry =>
                {
                    var key = new Tuple<string, GameObjectTypeEnum, string>(entry.Id, entry.GameObjectType, entry.AssetBundleName);

                    if (key.Item2 != GameObjectTypeEnum.AdvancedJSONMerge)
                    {
                        if (manifest.ManifestEntriesById.ContainsKey(key) && !manifestEntryStackById.ContainsKey(key))
                        {
                            manifestEntryStackById[key] = new List<ManifestEntry> {manifest.ManifestEntriesById[key]};
                            manifestEntryStackById[key].Add(entry);
                        }
                        else if (!manifest.ManifestEntriesById.ContainsKey(key) && !manifestEntryStackById.ContainsKey(key))
                        {
                            manifestEntryStackById[key] = new List<ManifestEntry> {entry};
                        }
                        else
                        {
                            manifestEntryStackById[key].Add(entry);
                        }
                    }
                    else
                    {
                        var mergeSpec = entry.Json;
                        var targetIds = new List<string>();
                        if (mergeSpec["TargetID"] != null)
                        {
                            targetIds.Add(mergeSpec["TargetID"].ToString());
                        }
                        else
                        {
                            targetIds.AddRange(((JArray) mergeSpec["TargetIDs"]).Select(token => token.ToString()));
                        }

                        targetIds.ForEach(targetId =>
                        {
                            var keys = manifestEntryStackById.Keys.Where(tuple => tuple.Item1 == targetId).Union(manifest.ManifestEntriesById.Keys.Where(tuple => tuple.Item1 == targetId)).ToList();
                            keys.ForEach(tuple =>
                            {
                                key = tuple;
                                if (manifest.ManifestEntriesById.ContainsKey(key) && !manifestEntryStackById.ContainsKey(key))
                                {
                                    manifestEntryStackById[key] = new List<ManifestEntry> {manifest.ManifestEntriesById[key]};
                                    manifestEntryStackById[key].Add(entry);
                                }
                                else
                                {
                                    manifestEntryStackById[tuple].Add(entry);
                                }
                            });
                        });
                    }
                });
            });

            // Add any manifest entries that haven't been touched by mods...
            manifest.ManifestEntriesById.Where(pair => !manifestEntryStackById.ContainsKey(pair.Key)).ToList().ForEach(pair => { manifestEntryStackById[pair.Key] = new List<ManifestEntry> {pair.Value}; });

            return manifestEntryStackById;
        }
    }
}