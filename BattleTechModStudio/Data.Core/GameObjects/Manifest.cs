using System;
using System.Collections.Generic;
using System.IO;
using Data.Core.Constants;
using Data.Core.Enums;
using Data.Core.Parsers;
using Data.Core.Scrapers;

namespace Data.Core.GameObjects
{
    public class Manifest
    {
        private List<ManifestEntry> _baseManifestEntries;
        private List<ManifestEntry> _dlcManifestEntries;

        public Dictionary<Tuple<string, GameObjectTypeEnum, string>, ManifestEntry> ManifestEntriesById = new Dictionary<Tuple<string, GameObjectTypeEnum, string>, ManifestEntry>();

        public void InitBaseManifest(string battleTechDirectory)
        {
            var streamingAssetDirectoryInfo = new DirectoryInfo(Path.Combine(battleTechDirectory, GameConstants.StreamingAssetsDirectory));
            if (!streamingAssetDirectoryInfo.Exists)
            {
                Console.WriteLine($"Warning: Streaming assets directory [{streamingAssetDirectoryInfo.FullName}] does not exist.");
                return;
            }

            var dataDirectory = new DirectoryInfo(Path.Combine(streamingAssetDirectoryInfo.FullName, GameConstants.DataDirectory));
            _baseManifestEntries = VersionManifestParser.ParseVersionManifest(streamingAssetDirectoryInfo.FullName, Path.Combine(dataDirectory.FullName, GameConstants.VersionManifestFilename));
            _baseManifestEntries.ForEach(entry => { ManifestEntriesById[new Tuple<string, GameObjectTypeEnum, string>(entry.Id, entry.GameObjectType, entry.AssetBundleName ?? string.Empty)] = entry; });
        }

        public void InitDlcManifest(string dlcDataDirectory)
        {
            var knownDlc = new List<string>
            {
                "Flashpoint",
                "UrbanWarfare",
                "HeavyMetal"
            };

            // TODO : Check which DLC are installed and constrain to installed DLC?
            _dlcManifestEntries = new List<ManifestEntry>();
            knownDlc.ForEach(s => { _dlcManifestEntries.AddRange(StreamingAssetsScraper.ScrapeStreamingAssets(Path.Combine(dlcDataDirectory, s))); });
            _dlcManifestEntries.ForEach(entry =>
            {
                if (ManifestEntriesById.ContainsKey(new Tuple<string, GameObjectTypeEnum, string>(entry.Id, entry.GameObjectType, entry.AssetBundleName)))
                {
                    Console.WriteLine($"DLC overwriting original base def for [{entry.Id}]");
                }

                ManifestEntriesById[new Tuple<string, GameObjectTypeEnum, string>(entry.Id, entry.GameObjectType, entry.AssetBundleName ?? string.Empty)] = entry;
            });
        }
    }
}