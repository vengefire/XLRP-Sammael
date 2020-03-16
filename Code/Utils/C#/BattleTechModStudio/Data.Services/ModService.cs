using System.IO;
using System.Linq;
using Data.Core.Constants;
using Data.Core.ModObjects;
using Data.Interfaces;

namespace Data.Services
{
    public class ModService : IModService
    {
        public ModCollection LoadModCollectionFromDirectory(string sourceDirectory)
        {
            var modCollection = new ModCollection();
            Directory.EnumerateDirectories(sourceDirectory)
                .Where(directory => File.Exists(Path.Combine(directory, ModObjectConstants.ModFileName)))
                .ToList().ForEach(
                    directory => { modCollection.AddMod(LoadModBaseFromDirectory(directory)); }
                );

            modCollection.ProcessModDependencies();
            modCollection.ProcessModLoadOrder();

            return modCollection;
        }

        private Mod LoadModBaseFromDirectory(string directory)
        {
            var modFilePath = Path.Combine(directory, ModObjectConstants.ModFileName);
            var mod = Mod.FromJson(File.ReadAllText(modFilePath));
            mod.ModDirectory = new DirectoryInfo(directory);
            return mod;
        }

        public void PublishLoadResults(ModCollection modCollection)
        {
            var disabledMods = modCollection.DisabledMods.ToList();
            var validMods = modCollection.ValidMods.ToList();
            var invalidMods = modCollection.InvalidMods.ToList();

            System.Console.WriteLine($"Summary for mods loaded:\r\n" +
                                     $"Total Mods Found - {modCollection.Mods.Count}\r\n" +
                                     $"Disabled Mods - {disabledMods.Count}\r\n" +
                                     $"Valid Mods Loaded - {validMods.Count}\r\n" +
                                     $"Invalid Mods - {invalidMods.Count}\r\n");

            System.Console.WriteLine("Disabled Mods:");
            disabledMods.ForEach(mod => { System.Console.WriteLine($"{mod.Name}"); });
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
        }
    }
}