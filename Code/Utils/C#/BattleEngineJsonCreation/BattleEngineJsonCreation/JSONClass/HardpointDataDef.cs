using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleEngineJsonCreation
{
    public partial class HardpointDataDef
    {
        [JsonProperty("ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("HardpointData", NullValueHandling = NullValueHandling.Ignore)]
        public List<HardpointDatum> HardpointData { get; set; }
    }

    public partial class HardpointDatum
    {
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public Location? Location { get; set; }

        [JsonProperty("weapons", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<string>> Weapons { get; set; }

        [JsonProperty("blanks", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Blanks { get; set; }

        [JsonProperty("mountingPoints", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> MountingPoints { get; set; }
    }
    public partial class HardpointDataDef
    {
        public static HardpointDataDef FromJson(string json) => JsonConvert.DeserializeObject<HardpointDataDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }
}
