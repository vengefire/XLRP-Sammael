using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.Core.GameObjects
{
    public partial class ComponentTags
    {
        [JsonProperty("items")]
        public List<string> Items { get; set; }

        [JsonProperty("tagSetSourceFile")]
        public string TagSetSourceFile { get; set; }
    }
}