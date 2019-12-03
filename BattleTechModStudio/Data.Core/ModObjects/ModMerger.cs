using System;
using System.Collections.Generic;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;

namespace Data.Core.ModObjects
{
    public class ModMerger
    {
        public static (List<ManifestEntry> mergedManifestEntries, Dictionary<Tuple<string, GameObjectTypeEnum, string>, List<ManifestEntry>> manifestEntryStackById) Merge(Manifest manifest, ModCollection modCollection)
        {
            var manifestEntryStackById = ModMerger.BuildMergeStack(manifest, modCollection);
            var mergedManifestEntries = ModMerger.ProcessMergeStack(manifestEntryStackById);
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
                    mergedResult = manifestEntryStackItem.Value.First();
                }
                else
                {
                    Console.WriteLine($"Merging {manifestEntryStackItem.Key}...");
                    foreach (var currentManifestEntry in manifestEntryStackItem.Value)
                    {
                        if (mergedResult == null)
                        {
                            mergedResult = currentManifestEntry;
                            Console.WriteLine($"Initialize merge result to {currentManifestEntry.Id} - {currentManifestEntry.GameObjectType} - {currentManifestEntry.AssetBundleName} - {currentManifestEntry.DirectoryInfo?.FullName}");
                            continue;
                        }

                        if (currentManifestEntry.ManifestEntryGroup == null)
                        {
                            Console.WriteLine($"Overwrite merge result to {currentManifestEntry.Id} - {currentManifestEntry.GameObjectType} - {currentManifestEntry.AssetBundleName} - {currentManifestEntry.DirectoryInfo?.FullName} - No Manifest Group.");
                            mergedResult = currentManifestEntry;
                            continue;
                        }

                        if (currentManifestEntry.ManifestEntryGroup.ShouldAppendText)
                        {
                            var targetText = mergedResult.Text;
                            var appendText = currentManifestEntry.Text;
                            mergedResult.Text = targetText += appendText;
                            Console.WriteLine($"Appended Text result with {currentManifestEntry.Id} - {currentManifestEntry.GameObjectType} - {currentManifestEntry.AssetBundleName} - {currentManifestEntry.DirectoryInfo?.FullName} - ShouldAppendText specified.");
                            continue;
                        }

                        if (!currentManifestEntry.ManifestEntryGroup.ShouldMergeJson)
                        {
                            Console.WriteLine($"Overwrite merge result to {currentManifestEntry.Id} - {currentManifestEntry.GameObjectType} - {currentManifestEntry.AssetBundleName} - {currentManifestEntry.DirectoryInfo?.FullName} - Manifest Group specifies overwrite.");
                            mergedResult = currentManifestEntry;
                            continue;
                        }

                        if (currentManifestEntry.GameObjectType != GameObjectTypeEnum.AdvancedJSONMerge)
                        {
                            var targetJson = mergedResult.Json;
                            var mergeJson = currentManifestEntry.Json;
                            targetJson.Merge(mergeJson);
                            mergedResult.Json = targetJson;
                            Console.WriteLine($"Merged result with {currentManifestEntry.Id} - {currentManifestEntry.GameObjectType} - {currentManifestEntry.AssetBundleName} - {currentManifestEntry.DirectoryInfo?.FullName} - Basic Merge specified.");
                        }
                    }
                }

                mergedManifestEntries.Add(mergedResult);
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
                });
            });

            // Add any manifest entries that haven't been touched by mods...
            manifest.ManifestEntriesById.Where(pair => !manifestEntryStackById.ContainsKey(pair.Key)).ToList().ForEach(pair =>
            {
                manifestEntryStackById[pair.Key] = new List<ManifestEntry> {pair.Value};
            });

            return manifestEntryStackById;
        }
    }
}