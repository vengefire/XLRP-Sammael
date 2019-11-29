using System;
using System.IO;
using Data.Core.Constants;
using Data.Core.Parsers;

namespace Data.Core.GameObjects
{
    public class Manifest
    {
        public void InitBaseManifest(string battleTechDirectory)
        {
            var streamingAssetDirectoryInfo = new DirectoryInfo(Path.Combine(battleTechDirectory, GameConstants.StreamingAssetsDirectory));
            if (!streamingAssetDirectoryInfo.Exists)
            {
                Console.WriteLine($"Warning: Streaming assets directory [{streamingAssetDirectoryInfo.FullName}] does not exist.");
                return;
            }

            var dataDirectory = new DirectoryInfo(Path.Combine(streamingAssetDirectoryInfo.FullName, GameConstants.DataDirectory));
            var baseManifestEntries = VersionManifestParser.ParseVersionManifest(dataDirectory.FullName, Path.Combine(dataDirectory.FullName, GameConstants.VersionManifestFilename));
        }

        public void InitDlcManifest(string dlcDataDirectory)
        {
            throw new NotImplementedException();
        }
    }
}