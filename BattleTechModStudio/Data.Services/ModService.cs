using System.IO;
using System.Linq;
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
            modCollection.ExpandManifests();
            return modCollection;
        }

        private Mod LoadModBaseFromDirectory(string directory)
        {
            var modFilePath = Path.Combine(directory, ModObjectConstants.ModFileName);
            var mod = Mod.FromJson(File.ReadAllText(modFilePath));
            mod.ModDirectory = new DirectoryInfo(directory);
            return mod;
        }
    }
}