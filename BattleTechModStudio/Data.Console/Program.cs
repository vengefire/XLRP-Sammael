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
                                     $"Invalid Mods - {modCollection.Mods.Count(mod => !mod.IsValid)}\r\n" +
                                     $"Press any key to continue...");
            System.Console.ReadKey();
        }
    }
}