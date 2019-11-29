using Data.Core.GameObjects;

namespace Data.Services
{
    public class ManifestService
    {
        public Manifest InitManifestFromDisk(string battleTechDirectory, string dlcDataDirectory)
        {
            var manifest = new Manifest();
            manifest.InitBaseManifest(battleTechDirectory);
            // manifest.InitDlcManifest(dlcDataDirectory);
            return manifest;
        }
    }
}