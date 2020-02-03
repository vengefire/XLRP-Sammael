using Newtonsoft.Json;

namespace Data.Core.GameObjects
{
    public partial class AdditionalDatum
    {
        [JsonProperty("statName")]
        public string StatName { get; set; }

        [JsonProperty("statValue")]
        public string StatValue { get; set; }

        [JsonProperty("statType")]
        public string StatType { get; set; }
    }
}