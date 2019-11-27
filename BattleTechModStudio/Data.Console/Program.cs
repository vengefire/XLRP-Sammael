using System.Linq;
using Data.Services;

namespace Data.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sourceDirectory = @"C:\Users\Stephen Weistra\gitrepos\BEX-CE";

            var modService = new ModService();
            var modCollection = modService.LoadModCollectionFromDirectory(sourceDirectory);

            System.Console.WriteLine($"Summary for mods loaded from [{sourceDirectory}]:\r\n" +
                                     $"Mods Loaded - {modCollection.Mods.Count}\r\n" +
                                     $"Invalid Mods - {modCollection.Mods.Count(mod => !mod.IsValid)}\r\n");

            modCollection.Mods.Where(mod => !mod.IsValid).ToList().ForEach(mod =>
            {
                System.Console.WriteLine($"Mod [{mod.Name}] is invalid for the following reasons:");
                System.Console.WriteLine($"{string.Join("\r\n", mod.InvalidReasonList)}\r\n");
            });

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}