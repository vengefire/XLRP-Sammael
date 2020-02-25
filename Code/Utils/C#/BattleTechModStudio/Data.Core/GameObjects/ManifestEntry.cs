using System.IO;
using Data.Core.Enums;
using Data.Core.ModObjects;
using Newtonsoft.Json.Linq;

namespace Data.Core.GameObjects
{
    public class ManifestEntry
    {
        private JObject _json;
        private string _text;

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

        public JObject Json
        {
            get
            {
                if (_json == null)
                {
                    _json = ReadJson();
                }

                return _json;
            }
            set => _json = value;
        }

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _text = ReadText();
                }

                return _text;
            }
            set => _text = value;
        }

        public JObject ReadJson()
        {
            return JsonUtils.DeserializeJson(FileInfo.FullName);
        }

        public string ReadText()
        {
            return File.ReadAllText(FileInfo.FullName);
        }

        public void ClearContent()
        {
            Json = null;
            Text = null;
        }
    }
}