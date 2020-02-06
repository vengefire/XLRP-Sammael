using System;
using System.IO;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects.Gear;
using Data.Core.Misc;
using Data.Core.ModObjects;
using Data.Core.Parsers;
using Data.Services;

namespace Data.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sourceDirectory = @"C:\Users\Stephen Weistra\gitrepos\BEX-CE";
            //var sourceDirectory = @"C:\Users\Stephen Weistra\gitrepos\RogueTech";
            //var sourceDirectory = @"D:\XLRP Fixes\XLRP - Reference - 20190725 - With CAB";
            var btDirectory = @"D:\Test Data\BT Base Data";
            var dlcDirectory = @"C:\Users\Stephen Weistra\gitrepos\bt-dlc-designdata";

            var manifestService = new ManifestService();
            var manifest = manifestService.InitManifestFromDisk(btDirectory, dlcDirectory);

            System.Console.WriteLine("Unknown types = \r\n" +
                                     $"{string.Join("\r\n", VersionManifestParser.UnknownTypes.ToList())}");

            var modService = new ModService();
            var modCollection = modService.LoadModCollectionFromDirectory(sourceDirectory);

            var disabledMods = modCollection.DisabledMods.ToList();
            var validMods = modCollection.ValidMods.ToList();
            var invalidMods = modCollection.InvalidMods.ToList();

            System.Console.WriteLine($"Summary for mods loaded from [{sourceDirectory}]:\r\n" +
                                     $"Total Mods Found - {modCollection.Mods.Count}\r\n" +
                                     $"Disabled Mods - {disabledMods.Count}\r\n" +
                                     $"Valid Mods Loaded - {validMods.Count}\r\n" +
                                     $"Invalid Mods - {invalidMods.Count}\r\n");

            System.Console.WriteLine("Disabled Mods:");
            disabledMods.ForEach(mod =>
            {
                System.Console.WriteLine($"{mod.Name}");
            });
            System.Console.WriteLine();

            System.Console.WriteLine("Invalid Mods:");
            invalidMods.ForEach(mod =>
            {
                System.Console.WriteLine($"{mod.Name}\r\n" +
                                         $"\t{string.Join("\r\n", mod.InvalidReasonList)}");
            });
            System.Console.WriteLine();

            System.Console.WriteLine("Valid Mods Load Order : ");
            validMods.GroupBy(mod => mod.LoadCycle).OrderBy(mods => mods.Key).ToList().ForEach(mods =>
            {
                System.Console.WriteLine($"Load Cycle [{mods.Key}]:\r\n" +
                                         $"{new string('-', 10)}\r\n" +
                                         $"{string.Join("\r\n", mods.OrderBy(mod => mod.LoadOrder).Select(mod => $"[{mod.LoadOrder,-3}] - {mod.Name}"))}\r\n");
            });

            var typeUnion = VersionManifestParser.AllTypes;
            modCollection.Mods.SelectMany(mod => mod.ManifestEntryGroups.Select(group => group.Type)).Distinct().ToList().ForEach(s => typeUnion.Add(s));
            var sortedTypes = typeUnion.ToList();
            sortedTypes.Sort(string.CompareOrdinal);
            var typeEnumString = TypeEnumGenerator.GenerateEnum(
                sortedTypes,
                "",
                "GameObjectTypeEnum"
            );

            System.Console.WriteLine("Generated Type Enum:\r\n" +
                                     $"{typeEnumString}");

            modCollection.ExpandManifestGroups();

            var result = ModMerger.Merge(manifest, modCollection);

            System.Console.WriteLine($"Failed Merges : \r\n" +
                                     $"{string.Join("\r\n", ModMerger.FailedMerges.Select(tuple => $"{tuple.Item1.FileInfo.FullName} - {tuple.Item2}"))}");

            /*var modifiedIds = result.manifestEntryStackById.Where(pair => pair.Value.Count > 1);
            System.Console.WriteLine($"Ids modified multiple times via modifications:\r\n" +
                                     $"{string.Join("\r\n", modifiedIds.Select(pair => $"\t{pair.Key} - {pair.Value.Count}"))}");*/

            var weapons = result.mergedManifestEntries.Where(entry => entry.GameObjectType == GameObjectTypeEnum.WeaponDef).Select(entry => WeaponBase.FromJson(entry.Json.ToString())).ToList();
            weapons.Sort((weapon1, weapon2) => string.CompareOrdinal(weapon1.Description.Id, weapon2.Description.Id));
            using (var file = File.CreateText(@"c:\tmp\btms-weapons.csv"))
            {
                file.WriteLine($"Description.Id|Category|AttackRecoil|weapon.AccuracyModifier|weapon.AmmoCategory|weapon.AoeCapable|" +
                               $"weapon.BonusValueA|weapon.BonusValueB|weapon.CanExplode|weapon.Category|weapon.CriticalChanceMultiplier|" +
                               $"weapon.Damage|weapon.DamageVariance|weapon.EvasiveDamageMultiplier|weapon.EvasivePipsIgnored|weapon.HeatDamage|weapon.HeatGenerated|" +
                               $"weapon.IndirectFireCapable|weapon.Instability|weapon.InventorySize|weapon.MaxRange|weapon.MinRange|weapon.OverheatedDamageMultiplier|" +
                               $"weapon.ProjectilesPerShot|weapon.RefireModifier|weapon.ShotsWhenFired|weapon.StartingAmmoCapacity|weapon.RangeSplit|" +
                               $"weapon.WeaponSubType");
                foreach (var weapon in weapons)
                {
                    file.WriteLine($"{weapon.Description.Id}|{weapon.Category}|{weapon.AttackRecoil}|{weapon.AccuracyModifier}|{weapon.AmmoCategory}|{weapon.AoeCapable}|" +
                                   $"'{weapon.BonusValueA}|'{weapon.BonusValueB}|{weapon.CanExplode}|{weapon.Category}|{weapon.CriticalChanceMultiplier}|" +
                                   $"{weapon.Damage}|{weapon.DamageVariance}|{weapon.EvasiveDamageMultiplier}|{weapon.EvasivePipsIgnored}|{weapon.HeatDamage}|{weapon.HeatGenerated}|" +
                                   $"{weapon.IndirectFireCapable}|{weapon.Instability}|{weapon.InventorySize}|{weapon.MaxRange}|{weapon.MinRange}|{weapon.OverheatedDamageMultiplier}|" +
                                   $"{weapon.ProjectilesPerShot}|{weapon.RefireModifier}|{weapon.ShotsWhenFired}|{weapon.StartingAmmoCapacity}|{string.Join(",", weapon.RangeSplit)}|" +
                                   $"{weapon.WeaponSubType}");
                }
            }
            /*var weapons = result.mergedManifestEntries.Where(entry => entry.GameObjectType == GameObjectTypeEnum.WeaponDef).Select(entry => $"{entry.Id} - {entry.GameObjectType} - {entry.AssetBundleName}").ToList();
            weapons.Sort();
            System.Console.WriteLine("Distinct Weapon Definitions:\r\n" +
                                     $"{string.Join("\r\n", weapons)}");

            System.Console.WriteLine($"Mechs!\r\n" +
                $"{string.Join("\r\n", result.mergedManifestEntries.Where(entry => entry.GameObjectType == GameObjectTypeEnum.MechDef).Select(entry => entry.Id))}");

            System.Console.WriteLine($"mechdef_annihilator_ANH-1A\r\n" +
                                     $"{result.mergedManifestEntries.First(entry => entry.Id == "mechdef_annihilator_ANH-1A").Json}");*/

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}