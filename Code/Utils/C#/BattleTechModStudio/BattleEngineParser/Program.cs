using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleEngineParser.Enums;
using BattleEngineParser.Models;
using Data.Core.Enums;
using Data.Core.ModObjects;
using Data.Services;

namespace BattleEngineParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            args.ToList().ForEach(s =>
            {
                if (!Directory.Exists(s))
                {
                    throw new InvalidProgramException($"Specified directory [{s}] cannot be found.");
                }
            });

            var designsDirectory = args[0];
            var btDirectory = args[1];
            var dlcDirectory = args[2];
            var sourceDirectory = args[3];

            // Load all Battle Engine designs available from the specified directory... 
            var designFiles = Directory.EnumerateFiles(designsDirectory, "*.bed", SearchOption.AllDirectories);
            var mechDesigns = new List<MechDesign>();

            designFiles
                .ToList()
                .ForEach(filePath =>
                {
                    var mechDesign = MechDesign.MechDesignFromFile(filePath);
                    mechDesigns.Add(mechDesign);
                });

            Console.WriteLine($"Processed [{mechDesigns.Count()}] designs...");

            // Load the base HBS BT manifest...
            var manifestService = new ManifestService();
            var manifest = manifestService.InitManifestFromDisk(btDirectory, dlcDirectory);

            // Load the Mod Collection resources...
            var modService = new ModService();
            var modCollection = modService.LoadModCollectionFromDirectory(sourceDirectory);
            modService.PublishLoadResults(modCollection);
            modCollection.ExpandManifestGroups();

            // Merge the base manifest with the mod collection resources
            var result = ModMerger.Merge(manifest, modCollection);
            var groupedData = result.mergedManifestEntries.GroupBy(entry => entry.GameObjectType, entry => entry,
                (type, entries) => new {GameObjectType = type, objects = entries});

            // Build up a handy-dandy type dictionary from the merged manifests...
            var typeDictionary = groupedData
                .ToDictionary(arg => arg.GameObjectType, arg => arg.objects.ToList());

            // Build up a handy-dandy list of mech asset bundles from chassis data...
            var chassisUsedAssetBundles = typeDictionary[GameObjectTypeEnum.ChassisDef]
                .Select(entry => entry.Json["PrefabIdentifier"].ToString().ToLower()).Distinct();

            var foundAssetBundles = typeDictionary[GameObjectTypeEnum.AssetBundle].Select(entry => entry.Id.ToLower());

            var unionAssetBundles = chassisUsedAssetBundles.Union(foundAssetBundles).Distinct().ToList();

            // Console.WriteLine(string.Join("\r\n", unionAssetBundles));

            System.Console.WriteLine("Failed Merges : \r\n" +
                                     $"{string.Join("\r\n", ModMerger.FailedMerges.Select(tuple => $"{tuple.Item1.FileInfo.FullName} - {tuple.Item2}"))}");

            var filterEras = new List<Era>()
            {
                Era.Star_League,
                Era.EarlySuccessionWar,
                Era.LateSuccessionWarLosTech,
                Era.LateSuccessionWarRenaissance,
                Era.Clan_Invasion
            };

            var filterTech = new List<TechLevel>()
            {
                TechLevel.IS
            };

            var filterChassisType = new List<ChassisType>()
            {
                ChassisType.Biped
            };

            var filteredDesigns =
                mechDesigns
                    .Where(design => design.Eras.Any(data => filterEras.Contains(data.EraDetail.Era)))
                    .Where(design => filterTech.Contains(design.TechLevel))
                    .Where(design => filterChassisType.Contains(design.ChassisType))
                    .ToList();

            Console.WriteLine(
                $"Filtered designs from [{mechDesigns.Count()}] to [{filteredDesigns.Count()}]. Filtering by available asset bundles...");

            var assetFilteredDesigns = filteredDesigns
                .Where(design => unionAssetBundles.Any(s =>
                    s.Contains(design.InnerSphereChassisDesignation.ToLower()) ||
                    (!string.IsNullOrEmpty(design.FilthyClanChassisDesignation) && s.Contains(design.FilthyClanChassisDesignation.ToLower()))))
                .ToList();
            
            Console.WriteLine($"Found [{assetFilteredDesigns.Count}] designs with dedicated asset bundles, filtering by chassis defs...");

            var chassisFilteredDesigns = assetFilteredDesigns
                .Where(design => typeDictionary[GameObjectTypeEnum.ChassisDef]
                    .Where(entry => (entry.Id.ToLower().Contains(design.InnerSphereChassisDesignation.ToLower()) ||
                                     (!string.IsNullOrEmpty(design.FilthyClanChassisDesignation) &&
                                      entry.Id.ToLower().Contains(design.FilthyClanChassisDesignation.ToLower()))))
                    .Any(entry =>
                    {
                        var id = entry.Id.ToLower();
                        var parts = id.ToLower().Split('_');
                        if (!parts.Contains(design.VariantDesignation.ToLower()))
                        {
                            return false;
                        }
                        
                        // Must contain all hero designations if any exist
                        var missingHeroDesignations = design.HeroDesignation.Except(parts);
                        if (missingHeroDesignations.Any())
                        {
                            return false;
                        }

                        return true;
                    }));
            
            Console.WriteLine($"Found [{chassisFilteredDesigns.Count()}] designs with dedicated chassis defs...");
        }
    }
}