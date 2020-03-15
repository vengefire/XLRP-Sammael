using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var typeDictionary = groupedData
                .ToDictionary(arg => arg.GameObjectType, arg => arg.objects.ToList());
            
            Console.WriteLine(string.Join("\r\n", typeDictionary[GameObjectTypeEnum.AssetBundle].Select(entry => entry.Id)));
            
            System.Console.WriteLine("Failed Merges : \r\n" +
                                     $"{string.Join("\r\n", ModMerger.FailedMerges.Select(tuple => $"{tuple.Item1.FileInfo.FullName} - {tuple.Item2}"))}");

            var designsWithPrefabs = mechDesigns.Where(design =>
                typeDictionary[GameObjectTypeEnum.AssetBundle]
                    .Any(entry => entry.Id.Contains(design.InnerSphereChassisDesignation)));
            
            Console.WriteLine($"Filtered mech designs to [{designsWithPrefabs.Count()}] designs with prefab support...");
        }
    }
}