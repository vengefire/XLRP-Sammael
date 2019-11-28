using System.Linq;
using Data.Console.Utils;
using Data.Core.Enums;
using Data.Services;

namespace Data.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // var sourceDirectory = @"C:\Users\Stephen Weistra\gitrepos\BEX-CE";
            var sourceDirectory = @"D:\XLRP Fixes\XLRP - Reference - 20190725 - With CAB";

            var modService = new ModService();
            var modCollection = modService.LoadModCollectionFromDirectory(sourceDirectory);

            var validMods = modCollection.ValidMods.ToList();
            var invalidMods = modCollection.InvalidMods.ToList();

            System.Console.WriteLine($"Summary for mods loaded from [{sourceDirectory}]:\r\n" +
                                     $"Mods Loaded - {validMods.Count}\r\n" +
                                     $"Invalid Mods - {invalidMods.Count}\r\n");

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

            var typeEnumString = TypeEnumGenerator.GenerateEnum(
                TypeEnumGenerator.GetUniqueTypes(
                    modCollection.ValidMods.SelectMany(mod => mod.ManifestEntryGroups.Select(entry => entry)).ToList()
                ).ToList(),
                "",
                "GameObjectTypeEnum"
            );

            System.Console.WriteLine($"Generated Type Enum:\r\n" +
                                     $"{typeEnumString}");

            // TODO: Solve the null list from JSON source issue. Default value to empty list?
            var weapons = modCollection.Mods.SelectMany(mod => mod.ManifestEntries().Where(entry => entry.GameObjectType == GameObjectTypeEnum.WeaponDef).Select(entry => entry.Id)).Distinct();
            System.Console.WriteLine($"Distinct Weapon Definitions:\r\n" +
                                     $"{string.Join("\r\n", weapons)}");

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}