using System.Collections.Generic;
using System.Linq;
using Data.Core.GameObjects;

namespace Data.Core.ModObjects
{
    public class ModMerger
    {
        public static List<ManifestEntry> Merge(Manifest manifest, ModCollection modCollection)
        {
            var mergedManifestEntries = new List<ManifestEntry>();
            var manifestEntryStackById = new Dictionary<string, List<ManifestEntry>>();
            modCollection.ValidModsLoadOrder.ForEach(mod =>
            {
                mod.ManifestEntries().ToList().ForEach(entry =>
                {
                    if (manifest.manifestEntriesById.ContainsKey(entry.Id) && !manifestEntryStackById.ContainsKey(entry.Id))
                    {
                        manifestEntryStackById[entry.Id] = new List<ManifestEntry>() { manifest.manifestEntriesById[entry.Id] };
                        manifestEntryStackById[entry.Id].Add(entry);
                    }
                    else if (!manifest.manifestEntriesById.ContainsKey(entry.Id) && !manifestEntryStackById.ContainsKey(entry.Id))
                    {
                        manifestEntryStackById[entry.Id] = new List<ManifestEntry>() { entry };
                    }
                    else
                    {
                        manifestEntryStackById[entry.Id].Add(entry);
                    }
                });
            });

            return mergedManifestEntries;
        }
    }
}