using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;
using Data.Core.Misc;

namespace Data.Core.ModObjects
{
    public partial class ManifestEntryGroup
    {
        public GameObjectTypeEnum ManifestGroupObjectType { get; set; }

        //public List<FileInfo> ManifestSourceFiles { get; set; }
        public List<FileInfo> ManifestSourceFiles { get; set; }
        public List<ManifestEntry> ManifestEntries { get; set; } = new List<ManifestEntry>();

        public DirectoryInfo BaseDirectory { get; set; }
        public DirectoryInfo ContentDirectory { get; set; }

        public void Expand(DirectoryInfo baseDirectory)
        {
            BaseDirectory = baseDirectory;
            ContentDirectory = new DirectoryInfo(System.IO.Path.Combine(BaseDirectory.FullName, Path));
            ManifestGroupObjectType = (GameObjectTypeEnum) Enum.Parse(typeof(GameObjectTypeEnum), Type);
            if (!string.IsNullOrEmpty(AssetBundleName) || string.IsNullOrEmpty(AssetBundleName) && !ContentDirectory.Exists)
            {
                if (string.IsNullOrEmpty(AssetBundleName) && !ContentDirectory.Exists)
                {
                    Console.WriteLine($"Warning : Manifest Content Directory [{ContentDirectory.FullName}] does not exist.");
                }

                ManifestSourceFiles = new List<FileInfo>();
            }
            else
            {
                ManifestSourceFiles = ContentDirectory.GetFiles("*", SearchOption.AllDirectories).ToList();
            }

            if (ManifestGroupObjectType != GameObjectTypeEnum.Prefab)
            {
                if (!ManifestSourceFiles.Any())
                {
                    //throw new Exception($"ManifestGroup [{ManifestGroupObjectType} - {baseDirectory.FullName} - {ContentDirectory.Name}] contains no entries.");
                    Console.WriteLine($"ManifestGroup [{ManifestGroupObjectType}] - [{baseDirectory.FullName}] - [{ContentDirectory.Name}] contains no entries.");
                }
                ManifestSourceFiles.ForEach(info => ManifestEntries.Add(new ManifestEntry(ContentDirectory, info, ManifestGroupObjectType, System.IO.Path.GetFileNameWithoutExtension(info.Name), this, AssetBundleName)));
            }
            else
            {
                ManifestEntries.Add(new ManifestEntry(null, null, ManifestGroupObjectType, Utils.GetPrefabIdFromPath(Path), this, AssetBundleName));
            }
        }
    }
}