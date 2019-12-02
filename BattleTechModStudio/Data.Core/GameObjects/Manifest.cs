using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Constants;
using Data.Core.Parsers;
using Data.Core.Scrapers;

namespace Data.Core.GameObjects
{
    public class Manifest
    {
        private List<ManifestEntry> _baseManifestEntries;
        private List<ManifestEntry> _dlcManifestEntries;

        public Dictionary<string, ManifestEntry> manifestEntriesById = new Dictionary<string, ManifestEntry>();

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
            var duplicatedIds = _baseManifestEntries.GroupBy(entry => new {entry.Id, entry.GameObjectType}).Where(entries => entries.Count() > 1);
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
        }
    }
}