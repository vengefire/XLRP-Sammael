using Newtonsoft.Json;

namespace Data.Core.ModObjects
{
    public class ManifestBase
    {
        [JsonProperty("Type")] public string Type { get; set; }

        [JsonProperty("Path")] public string Path { get; set; }
    }
}