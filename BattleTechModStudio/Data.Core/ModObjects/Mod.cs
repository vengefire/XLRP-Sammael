using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.ModObjects.Generated;

namespace Data.Core.ModObjects
{
    public partial class Mod
    {
        public bool IsValid = false;

        public List<Mod> DependsOnMods { get; set; } = new List<Mod>();
        public List<Mod> ConflictsWithMods { get; set; } = new List<Mod>();
        public List<Mod> OptionallyDependsOnMods { get; set; } = new List<Mod>();
        public List<string> InvalidReasonList { get; } = new List<string>();
        public int LoadOrder { get; set; }

        public int LoadCycle { get; set; }
        public DirectoryInfo ModDirectory { get; set; }

        public IEnumerable<ManifestEntry> ManifestEntries() => ManifestEntryGroups.SelectMany(group => group.ManifestEntries);

        public void ExpandManifestEntries()
        {
            ManifestEntryGroups.ForEach(entry => entry.Expand(ModDirectory));
        }
    }
}