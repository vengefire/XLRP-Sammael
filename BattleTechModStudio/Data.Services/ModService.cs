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
                .AsParallel().ForAll(
                //.ToList().ForEach(
                    directory => { modCollection.AddMod(LoadModBaseFromDirectory(directory)); }
                    );
            modCollection.ProcessModDependencies();
            modCollection.ProcessModLoadOrder();
            return modCollection;
        }

        private ModBase LoadModBaseFromDirectory(string directory)
        {
            return ModBase.FromJson(File.ReadAllText(Path.Combine(directory, ModObjectConstants.ModFileName)));
        }
    }
}