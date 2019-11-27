using Newtonsoft.Json;

namespace Data.Core.GameObjects
{
    public partial class Description
    {
        [JsonProperty("Cost")]
        public long Cost { get; set; }

        [JsonProperty("Rarity")]
        public long Rarity { get; set; }

        [JsonProperty("Purchasable")]
        public bool Purchasable { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("UIName")]
        public string UiName { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Details")]
        public string Details { get; set; }

        [JsonProperty("Icon")]
        public string Icon { get; set; }
    }
}