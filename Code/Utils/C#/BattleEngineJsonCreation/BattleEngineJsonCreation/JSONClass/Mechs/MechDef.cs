using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleEngineJsonCreation
{
    public partial class MechDef
    {
        [JsonProperty("ChassisID", NullValueHandling = NullValueHandling.Ignore)]
        public string ChassisId { get; set; }

        [JsonProperty("HeraldryID")]
        public object HeraldryId { get; set; }

        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public DefDescription Description { get; set; }

        [JsonProperty("simGameMechPartCost", NullValueHandling = NullValueHandling.Ignore)]
        public long? SimGameMechPartCost { get; set; }

        [JsonProperty("Version", NullValueHandling = NullValueHandling.Ignore)]
        public double? Version { get; set; }

        [JsonProperty("Locations", NullValueHandling = NullValueHandling.Ignore)]
        public List<MechDefLocation> Locations { get; set; }

        [JsonProperty("inventory", NullValueHandling = NullValueHandling.Ignore)]
        public List<Inventory> Inventory { get; set; }

        [JsonProperty("MechTags", NullValueHandling = NullValueHandling.Ignore)]
        public Tags MechTags { get; set; }

        [JsonProperty("MinAppearanceDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? MinAppearanceDate { get; set; }
    }
    public partial class Inventory
    {
        [JsonProperty("MountedLocation", NullValueHandling = NullValueHandling.Ignore)]
        public Location? MountedLocation { get; set; }

        [JsonProperty("ComponentDefID", NullValueHandling = NullValueHandling.Ignore)]
        public string ComponentDefId { get; set; }

        [JsonProperty("ComponentDefType", NullValueHandling = NullValueHandling.Ignore)]
        public ComponentDefType? ComponentDefType { get; set; }

        [JsonProperty("HardpointSlot", NullValueHandling = NullValueHandling.Ignore)]
        public long? HardpointSlot { get; set; }

        [JsonProperty("DamageLevel", NullValueHandling = NullValueHandling.Ignore)]
        public string DamageLevel { get; set; }

        [JsonProperty("prefabName")]
        public string PrefabName { get; set; }

        [JsonProperty("hasPrefabName", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasPrefabName { get; set; }

        [JsonProperty("SimGameUID")]
        public string SimGameUid { get; set; }

        [JsonProperty("GUID")]
        public object Guid { get; set; }
    }

    public partial class MechDefLocation
    {
        [JsonProperty("DamageLevel", NullValueHandling = NullValueHandling.Ignore)]
        public DamageLevel? DamageLevel { get; set; }

        [JsonProperty("Location", NullValueHandling = NullValueHandling.Ignore)]
        public Location? Location { get; set; }

        [JsonProperty("CurrentArmor", NullValueHandling = NullValueHandling.Ignore)]
        public long? CurrentArmor { get; set; }

        [JsonProperty("CurrentRearArmor", NullValueHandling = NullValueHandling.Ignore)]
        public long? CurrentRearArmor { get; set; }

        [JsonProperty("CurrentInternalStructure", NullValueHandling = NullValueHandling.Ignore)]
        public long? CurrentInternalStructure { get; set; }

        [JsonProperty("AssignedArmor", NullValueHandling = NullValueHandling.Ignore)]
        public long? AssignedArmor { get; set; }

        [JsonProperty("AssignedRearArmor", NullValueHandling = NullValueHandling.Ignore)]
        public long? AssignedRearArmor { get; set; }
    }
    public partial class MechDef
    {
        public static MechDef FromJson(string json) => JsonConvert.DeserializeObject<MechDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }
}
