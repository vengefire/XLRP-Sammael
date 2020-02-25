using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace Data.Console.Utils
{
    internal static class ExtractionUtils
    {
        private static void ExtractTags(string id, JArray jArray, JObject shopdefJsonObject, List<string> requiredTags, List<string> excludedTags, List<string> validFactions)
        {
            var progressTags = new List<string> () {"planet_progress_1", "planet_progress_2", "planet_progress_3"};
            var ignoredRequiredTags = new List<string>() {"debug"};
            var ignoredRestrictedTags = new List<string>() {"debug", "planet_other_empty"};
            ignoredRequiredTags.AddRange(progressTags);
            ignoredRestrictedTags.AddRange(progressTags);
        
            foreach (JObject item in jArray)
            {
                if (item["ID"].ToString() == id)
                {
                    var required = (JObject) shopdefJsonObject["RequirementTags"];
                    var items = (JArray) required?["items"];
                    if (items != null)
                    {
                        foreach (string jToken in items)
                        {
                            if (!requiredTags.Contains(jToken))
                            {
                                requiredTags.Add(jToken);
                            }
                        }
                    }

                    var exclusions = (JObject) shopdefJsonObject["ExclusionTags"];
                    items = (JArray) exclusions?["items"];
                    if (items != null)
                    {
                        foreach (string jToken in items)
                        {
                            if (!excludedTags.Contains(jToken))
                            {
                                excludedTags.Add(jToken);
                            }
                        }
                    }
                }
            }
            ignoredRequiredTags.ForEach(s => requiredTags.Remove(s));
            ignoredRestrictedTags.ForEach(s => excludedTags.Remove(s));
        }

        public static void ExtractItemsByTypeToExcel(List<ManifestEntry> manifestEntries, string outputDirectory, string filename, List<GameObjectTypeEnum> typesQualifyingForStores, bool alwaysRecreate = false)
        {
            var shopDefinitions = manifestEntries.Where(manifestEntry => manifestEntry.GameObjectType == GameObjectTypeEnum.ShopDef).ToList();

            var itemsByCategory = manifestEntries.Where(entry => typesQualifyingForStores.Contains(entry.GameObjectType))
                .GroupBy(entry => entry.GameObjectType, (key, entries) => new {ItemType = key, items = entries.Select(entry => entry)})
                .ToList();

            var outputFileName = Path.Combine(outputDirectory, filename);

            if (File.Exists(outputFileName) && alwaysRecreate)
            {
                File.Delete(outputFileName);
            }

            var excelPackage = new ExcelPackage(new FileInfo(outputFileName));
            var workBook = excelPackage.Workbook;
            //var index = 0;
            ExcelWorksheet currentSheet = null;

            var index = 1;
            var columnHeadersString = "Id\tPrototype date|Faction\tProduction Date|Faction\tExtinction Date\tReintro|Faction\tCommon Date\tAvailability\tRequired PlanetTags (Any of)\tRestricted PlanetTags (Any of)\tNotes";
            var columnHeadersSplit = columnHeadersString.Split('\t').ToList();
            var columnHeadersDict = new Dictionary<string, int>();
            columnHeadersSplit.ForEach(s => columnHeadersDict[s] = index++);

            var mechList = MechModel.ProcessAvailabilityFile();

            foreach (var shopItemCategory in itemsByCategory)
            {
                System.Console.WriteLine($"{shopItemCategory.ItemType}");
                System.Console.WriteLine($"{string.Join("\r\n", shopItemCategory.items.Select(entry => entry.Id))}");

                currentSheet = workBook.Worksheets[shopItemCategory.ItemType.ToString()];
                if (currentSheet == null)
                {
                    currentSheet = workBook.Worksheets.Add(shopItemCategory.ItemType.ToString());
                    foreach (var colHeaderEntry in columnHeadersDict)
                    {
                        currentSheet.Cells[1, colHeaderEntry.Value].Value = colHeaderEntry.Key;
                    }
                }

                var currentItems = new List<(int, string, ExcelRange)>();
                for (var i = 2; i <= currentSheet.Dimension.Rows; i++)
                {
                    var value = currentSheet.Cells[i, columnHeadersDict["Id"]].Value;
                    if (!string.IsNullOrEmpty(value?.ToString()))
                    {
                        currentItems.Add((i, value.ToString(), currentSheet.Cells[i, 1, i, currentSheet.Dimension.Columns]));
                    }
                }

                var addedItemIds = shopItemCategory.items.Select(entry => entry.Id).Except(currentItems.Select(tuple => tuple.Item2)).ToList();
                var addedItems = shopItemCategory.items.Where(entry => addedItemIds.Contains(entry.Id)).ToList();
                var removedItems = currentItems.Select(tuple => tuple.Item2).Except(shopItemCategory.items.Select(entry => entry.Id)).ToList();

                if (removedItems.Any())
                {
                    var removedSheetName = $"{shopItemCategory.ItemType.ToString()}-REM";
                    if (workBook.Worksheets[removedSheetName] != null)
                    {
                        workBook.Worksheets.Delete(removedSheetName);
                    }

                    var removedSheet = workBook.Worksheets.Add(removedSheetName);
                    currentSheet.Cells[1, 1, 1, currentSheet.Dimension.Columns].Copy(removedSheet.Cells[1, 1, 1, currentSheet.Dimension.Columns]);

                    for (var removedIndex = 0; removedIndex < removedItems.Count; ++removedIndex)
                    {
                        var removedItemId = removedItems[removedIndex];
                        var removedEntry = currentItems.First(tuple => tuple.Item2 == removedItemId);
                        removedEntry.Item3.Copy(removedSheet.Cells[removedIndex + 2, 1, removedIndex + 2, removedEntry.Item3.Columns]);
                        currentSheet.DeleteRow(removedEntry.Item1);
                        currentItems.Remove(removedEntry);
                    }
                }

                var rowIndex = currentSheet.Dimension.Rows + 1;

                var rarityMap = new List<(int min, int max, string bracket)>
                {
                    (0, 1, "Common"),
                    (1, 2, "Uncommon"),
                    (2, 3, "VeryUncommon"),
                    (3, 4, "Rare"),
                    (4, 5, "VeryRare"),
                    (5, 6, "PracticallyExtinct"),
                    (6, int.MaxValue, "Extinct")
                };
                rarityMap.Reverse();

                currentItems.ForEach(currentItem =>
                {
                    var entry = shopItemCategory.items.First(manifestEntry => manifestEntry.Id == currentItem.Item2);
                    var itemRarity = Convert.ToInt32(entry.Json["Description"]?["Rarity"]?.ToString());
                    var mappedRarity = rarityMap.First(tuple => itemRarity < tuple.max && itemRarity >= tuple.min);

                    if (entry.Id.Contains("Template"))
                    {
                        mappedRarity = (-1, -1, "NA");
                    }
                    
                    var requiredTags = new List<string>();
                    var validFactions = new List<string>();
                    var excludedTags = new List<string>();
                    shopDefinitions.ForEach(manifestEntry =>
                    {
                        ExtractionUtils.ExtractTags(currentItem.Item2, (JArray) manifestEntry.Json["Inventory"], manifestEntry.Json, requiredTags, excludedTags, validFactions);
                        ExtractionUtils.ExtractTags(currentItem.Item2, (JArray) manifestEntry.Json["Specials"], manifestEntry.Json, requiredTags, excludedTags, validFactions);
                    });

                    DateTime? appearanceDate = null;
                    if (entry.GameObjectType == GameObjectTypeEnum.MechDef)
                    {
                        var minAppearanceDateString = entry.Json["MinAppearanceDate"]?.ToString();
                        if (!string.IsNullOrEmpty(minAppearanceDateString))
                        {
                            appearanceDate = DateTime.Parse(minAppearanceDateString);
                        }
                        else
                        {
                            var mechModelEntry = mechList.FirstOrDefault(model => model.Name.Trim('"') == entry.Json["Description"]?["UIName"]?.ToString());
                            if (mechModelEntry != null)
                            {
                                appearanceDate = new DateTime((int)mechModelEntry.Year, 1, 1);
                            }
                        }
                    }

                    currentSheet.Cells[currentItem.Item1, columnHeadersDict["Availability"]].Value = mappedRarity.bracket;
                    currentSheet.Cells[currentItem.Item1, columnHeadersDict["Required PlanetTags (Any of)"]].Value = string.Join("|", requiredTags);
                    currentSheet.Cells[currentItem.Item1, columnHeadersDict["Restricted PlanetTags (Any of)"]].Value = string.Join("|", excludedTags);
                    currentSheet.Cells[currentItem.Item1, columnHeadersDict["Common Date"]].Value = appearanceDate.ToString();
                    // currentSheet.Cells[currentItem.Item1, columnHeadersDict["Notes"]].Value = entry.FileInfo.FullName;
                });

                addedItems.ForEach(entry =>
                {
                    var requiredTags = new List<string>();
                    var validFactions = new List<string>();
                    var excludedTags = new List<string>();
                    shopDefinitions.ForEach(manifestEntry =>
                    {
                        ExtractionUtils.ExtractTags(entry.Id, (JArray) manifestEntry.Json["Inventory"], manifestEntry.Json, requiredTags, excludedTags, validFactions);
                        ExtractionUtils.ExtractTags(entry.Id, (JArray) manifestEntry.Json["Specials"], manifestEntry.Json, requiredTags, excludedTags, validFactions);
                    });

                    currentSheet.Cells[rowIndex, 1].Value = entry.Id;
                    var itemRarity = Convert.ToInt32(entry.Json["Description"]?["Rarity"]?.ToString());
                    var mappedRarity = rarityMap.First(tuple => itemRarity < tuple.max && itemRarity >= tuple.min);
                    currentSheet.Cells[rowIndex, columnHeadersDict["Availability"]].Value = mappedRarity.bracket;
                    currentSheet.Cells[rowIndex, columnHeadersDict["Required PlanetTags (Any of)"]].Value = string.Join("|", requiredTags);
                    currentSheet.Cells[rowIndex, columnHeadersDict["Restricted PlanetTags (Any of)"]].Value = string.Join("|", excludedTags);
                    // currentSheet.Cells[rowIndex, columnHeadersDict["Notes"]].Value = entry.FileInfo.FullName;
                    rowIndex += 1;
                });
            }

            excelPackage.Save();
        }
    }
}