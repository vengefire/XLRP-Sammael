using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;

namespace Data.Core.Parsers
{
    public static class VersionManifestParser
    {
        public static HashSet<string> AllTypes = new HashSet<string>();
        public static HashSet<string> UnknownTypes = new HashSet<string>();

        public static List<ManifestEntry> ParseVersionManifest(string dataDirectory, string versionManifestFilePath)
        {
            var manifestEntries = new List<ManifestEntry>();

            var dataDirectoryInfo = new DirectoryInfo(dataDirectory);
            var versionManifestInfo = new FileInfo(versionManifestFilePath);

            if (!versionManifestInfo.Exists)
            {
                Console.WriteLine($"Warning: VersionManifest file [{versionManifestInfo.FullName}] does not exist.");
                return manifestEntries;
            }

            using (var fileStream = versionManifestInfo.OpenRead())
            {
                using (var reader = new StreamReader(fileStream))
                {
                    // Init Column Headers
                    var lineData = reader.ReadLine();
                    var columnHeaders = lineData.Split(',').ToList();
                    var columnHeaderIndexDictionary = new Dictionary<string, int>();
                    var rowDictionary = new Dictionary<string, string>();

                    var index = 0;
                    columnHeaders.ForEach(s =>
                    {
                        columnHeaderIndexDictionary[s] = index++;
                        rowDictionary[s] = string.Empty;
                    });

                    var idIndex = columnHeaderIndexDictionary["id"];
                    var typeIndex = columnHeaderIndexDictionary["type"];
                    var pathIndex = columnHeaderIndexDictionary["path"];
                    var assetBundleNameIndex = columnHeaderIndexDictionary["assetBundleName"];

                    while (!reader.EndOfStream)
                    {
                        lineData = reader.ReadLine();
                        if (string.IsNullOrEmpty(lineData))
                        {
                            continue;
                        }

                        var rowValues = lineData.Split(',').ToList();

                        if (rowValues.Count < columnHeaders.Count)
                        {
                            continue;
                        }

                        var id = rowValues[idIndex];
                        var typeString = rowValues[typeIndex];
                        var pathString = rowValues[pathIndex];
                        var assetBundleString = rowValues[assetBundleNameIndex];

                        if (pathString.StartsWith("Assets/") /*&& string.IsNullOrEmpty(assetBundleString)*/)
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(typeString) || id == "id")
                        {
                            continue;
                        }

                        AllTypes.Add(typeString);
                        if (!Enum.TryParse(typeString, out GameObjectTypeEnum gameObjectType))
                        {
                            UnknownTypes.Add(typeString);
                        }

                        var fileInfo = gameObjectType == GameObjectTypeEnum.Prefab
                            ? null
                            : new FileInfo(Path.Combine(dataDirectory, pathString));
                        var contentDirectory = gameObjectType == GameObjectTypeEnum.Prefab ? null : fileInfo.Directory;

                        if (fileInfo != null && !fileInfo.Exists)
                        {
                            Console.WriteLine(
                                $"Warning - Manifest Entry file [{fileInfo.FullName}] does not exist. Skipping...");
                        }
                        else
                        {
                            // Don't add updated entries, we don't care about these as they're overwritten...
                            if (!manifestEntries.Any(entry => entry.Id == id && entry.GameObjectType == gameObjectType))
                            {
                                manifestEntries.Add(new ManifestEntry(contentDirectory, fileInfo, gameObjectType, id,
                                    null, assetBundleString));
                            }
                            else
                            {
                                Console.WriteLine($"Skipped adding duplicated entry for [{fileInfo.FullName}]");
                            }
                        }
                    }
                }
            }

            return manifestEntries;
        }
    }
}