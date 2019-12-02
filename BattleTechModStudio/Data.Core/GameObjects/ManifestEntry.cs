using System.IO;
using Data.Core.Enums;
using Data.Core.ModObjects;

namespace Data.Core.GameObjects
{
    public class ManifestEntry
    {
        public ManifestEntry(DirectoryInfo directoryInfo, FileInfo fileInfo, GameObjectTypeEnum gameObjectType, string id, ManifestEntryGroup manifestEntryGroup, string assetBundleName)
        {
            DirectoryInfo = directoryInfo;
            FileInfo = fileInfo;
            GameObjectType = gameObjectType;
            Id = id;
            ManifestEntryGroup = manifestEntryGroup;
            AssetBundleName = assetBundleName;
        }

        public FileInfo FileInfo { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public ManifestEntryGroup ManifestEntryGroup { get; set; }
        public string Id { get; set; }
        public GameObjectTypeEnum GameObjectType { get; set; }
        public string AssetBundleName { get; set; }
    }
}