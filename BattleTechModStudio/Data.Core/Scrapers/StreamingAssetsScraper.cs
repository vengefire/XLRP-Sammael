using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Constants;
using Data.Core.Enums;
using Data.Core.GameObjects;

namespace Data.Core.Scrapers
{
    public static class StreamingAssetsScraper
    {
        public static HashSet<string> UnknownPaths = new HashSet<string>();

        public static List<ManifestEntry> ScrapeStreamingAssets(string streamingAssetsPath)
        {
            var manifestEntries = new List<ManifestEntry>();

            var streamingAssetsDirectory = new DirectoryInfo(streamingAssetsPath);
            var pathToTypeDictionary = new Dictionary<string, GameObjectTypeEnum>
            {
                {"abilities", GameObjectTypeEnum.AbilityDef},
                {"ammunition", GameObjectTypeEnum.AmmunitionDef},
                {"ammunitionBox", GameObjectTypeEnum.AmmunitionBoxDef},
                {"chassis", GameObjectTypeEnum.ChassisDef},
                {"contracts", GameObjectTypeEnum.ContractOverride},
                {"events", GameObjectTypeEnum.SimGameEventDef},
                {"flashpoints", GameObjectTypeEnum.FlashpointDef},
                {"hardpoints", GameObjectTypeEnum.HardpointDataDef},
                {"itemCollections", GameObjectTypeEnum.ItemCollectionDef},
                {"jumpjets", GameObjectTypeEnum.JumpJetDef},
                {"mech", GameObjectTypeEnum.MechDef},
                {"milestoneSets", GameObjectTypeEnum.SimGameMilestoneDef},
                {"movement", GameObjectTypeEnum.MovementCapabilitiesDef},
                {"upgrades", GameObjectTypeEnum.UpgradeDef},
                {"vehicle", GameObjectTypeEnum.VehicleDef},
                {"vehicleChassis", GameObjectTypeEnum.VehicleChassisDef},
                {"weapon", GameObjectTypeEnum.WeaponDef}
            };

            var dataDirectory = new DirectoryInfo(Path.Combine(streamingAssetsPath, GameConstants.DataDirectory));
            dataDirectory.GetDirectories()
                .ToList().ForEach(info =>
                {
                    if (!pathToTypeDictionary.ContainsKey(info.Name))
                    {
                        Console.WriteLine($"Unrecognised streaming assets directory path [{info.Name}]");
                        StreamingAssetsScraper.UnknownPaths.Add(info.Name);
                    }
                    else
                    {
                        var gameObjectType = pathToTypeDictionary[info.Name];
                        StreamingAssetsScraper.ScrapeStreamingAssetDataSubDirectory(info, manifestEntries, gameObjectType);
                    }
                });

            return manifestEntries;
        }

        private static void ScrapeStreamingAssetDataSubDirectory(DirectoryInfo directoryInfo, List<ManifestEntry> manifestEntries, GameObjectTypeEnum gameObjectType)
        {
            directoryInfo.GetFiles()
                .ToList().ForEach(fileInfo => { manifestEntries.Add(new ManifestEntry(directoryInfo, fileInfo, gameObjectType, Path.GetFileNameWithoutExtension(fileInfo.Name), null, null)); });

            directoryInfo.GetDirectories()
                .ToList().ForEach(info => StreamingAssetsScraper.ScrapeStreamingAssetDataSubDirectory(info, manifestEntries, gameObjectType));
        }
    }
}