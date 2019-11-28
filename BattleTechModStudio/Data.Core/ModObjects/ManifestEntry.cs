using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Core.Enums;

namespace Data.Core.ModObjects
{
    public partial class ManifestEntry
    {
        public GameObjectTypeEnum ManifestObjectType { get; set; }
        public List<FileInfo> ManifestSourceFiles { get; set; }

        public DirectoryInfo BaseDirectory { get; set; }
        public DirectoryInfo ContentDirectory { get; set; }

        public void Expand(DirectoryInfo baseDirectory)
        {
            BaseDirectory = baseDirectory;
            ContentDirectory = new DirectoryInfo(System.IO.Path.Combine(BaseDirectory.FullName, Path));
            ManifestObjectType = (GameObjectTypeEnum) Enum.Parse(typeof(GameObjectTypeEnum), Type);
            if (!string.IsNullOrEmpty(AssetBundleName) || (string.IsNullOrEmpty(AssetBundleName) && !ContentDirectory.Exists))
            {
                if (string.IsNullOrEmpty(AssetBundleName) && !ContentDirectory.Exists)
                {
                    Console.WriteLine($"Warning : Manifest Content Directory [{ContentDirectory.FullName}] does not exist.");
                }

                ManifestSourceFiles = new List<FileInfo>();
            }
            else
            {
                ManifestSourceFiles = ContentDirectory.GetFiles().ToList();
            }
        }
    }
}