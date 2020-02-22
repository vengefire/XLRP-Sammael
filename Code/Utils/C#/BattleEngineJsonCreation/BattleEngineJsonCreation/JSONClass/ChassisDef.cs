using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleEngineJsonCreation
{
    public partial class ChassisDef
    {
        [JsonProperty("FixedEquipment", NullValueHandling = NullValueHandling.Ignore)]
        public List<FixedEquipment> FixedEquipment { get; set; }

        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public DefDescription Description { get; set; }

        [JsonProperty("MovementCapDefID", NullValueHandling = NullValueHandling.Ignore)]
        public string MovementCapDefId { get; set; }

        [JsonProperty("PathingCapDefID", NullValueHandling = NullValueHandling.Ignore)]
        public string PathingCapDefId { get; set; }

        [JsonProperty("HardpointDataDefID", NullValueHandling = NullValueHandling.Ignore)]
        public string HardpointDataDefId { get; set; }

        [JsonProperty("PrefabIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefabIdentifier { get; set; }

        [JsonProperty("PrefabBase", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefabBase { get; set; }

        [JsonProperty("Tonnage", NullValueHandling = NullValueHandling.Ignore)]
        public int? Tonnage { get; set; }

        [JsonProperty("InitialTonnage", NullValueHandling = NullValueHandling.Ignore)]
        public double? InitialTonnage { get; set; }

        [JsonProperty("weightClass", NullValueHandling = NullValueHandling.Ignore)]
        public string WeightClass { get; set; }

        [JsonProperty("BattleValue", NullValueHandling = NullValueHandling.Ignore)]
        public int? BattleValue { get; set; }

        [JsonProperty("Heatsinks", NullValueHandling = NullValueHandling.Ignore)]
        public int? Heatsinks { get; set; }

        [JsonProperty("TopSpeed", NullValueHandling = NullValueHandling.Ignore)]
        public double? TopSpeed { get; set; }

        [JsonProperty("TurnRadius", NullValueHandling = NullValueHandling.Ignore)]
        public double? TurnRadius { get; set; }

        [JsonProperty("MaxJumpjets", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxJumpjets { get; set; }

        [JsonProperty("Stability", NullValueHandling = NullValueHandling.Ignore)]
        public int? Stability { get; set; }

        [JsonProperty("StabilityDefenses", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> StabilityDefenses { get; set; }

        [JsonProperty("SpotterDistanceMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public int? SpotterDistanceMultiplier { get; set; }

        [JsonProperty("VisibilityMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public int? VisibilityMultiplier { get; set; }

        [JsonProperty("SensorRangeMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public int? SensorRangeMultiplier { get; set; }

        [JsonProperty("Signature", NullValueHandling = NullValueHandling.Ignore)]
        public int? Signature { get; set; }

        [JsonProperty("Radius", NullValueHandling = NullValueHandling.Ignore)]
        public int? Radius { get; set; }

        [JsonProperty("PunchesWithLeftArm", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PunchesWithLeftArm { get; set; }

        [JsonProperty("MeleeDamage", NullValueHandling = NullValueHandling.Ignore)]
        public int? MeleeDamage { get; set; }

        [JsonProperty("MeleeInstability", NullValueHandling = NullValueHandling.Ignore)]
        public int? MeleeInstability { get; set; }

        [JsonProperty("MeleeToHitModifier", NullValueHandling = NullValueHandling.Ignore)]
        public int? MeleeToHitModifier { get; set; }

        [JsonProperty("DFADamage", NullValueHandling = NullValueHandling.Ignore)]
        public int? DfaDamage { get; set; }

        [JsonProperty("DFAToHitModifier", NullValueHandling = NullValueHandling.Ignore)]
        public int? DfaToHitModifier { get; set; }

        [JsonProperty("DFASelfDamage", NullValueHandling = NullValueHandling.Ignore)]
        public int? DfaSelfDamage { get; set; }

        [JsonProperty("DFAInstability", NullValueHandling = NullValueHandling.Ignore)]
        public int? DfaInstability { get; set; }

        [JsonProperty("Locations", NullValueHandling = NullValueHandling.Ignore)]
        public List<ChassisDefLocation> Locations { get; set; }

        [JsonProperty("LOSSourcePositions", NullValueHandling = NullValueHandling.Ignore)]
        public List<LosPosition> LosSourcePositions { get; set; }

        [JsonProperty("LOSTargetPositions", NullValueHandling = NullValueHandling.Ignore)]
        public List<LosPosition> LosTargetPositions { get; set; }

        [JsonProperty("VariantName", NullValueHandling = NullValueHandling.Ignore)]
        public string VariantName { get; set; }

        [JsonProperty("ChassisTags", NullValueHandling = NullValueHandling.Ignore)]
        public Tags ChassisTags { get; set; }

        [JsonProperty("StockRole", NullValueHandling = NullValueHandling.Ignore)]
        public string StockRole { get; set; }

        [JsonProperty("YangsThoughts", NullValueHandling = NullValueHandling.Ignore)]
        public string YangsThoughts { get; set; }
    }
    public partial class ChassisDefLocation
    {
        [JsonProperty("Location", NullValueHandling = NullValueHandling.Ignore)]
        public Location? Location { get; set; }

        [JsonProperty("Hardpoints", NullValueHandling = NullValueHandling.Ignore)]
        public List<Hardpoint> Hardpoints { get; set; }

        [JsonProperty("Tonnage", NullValueHandling = NullValueHandling.Ignore)]
        public double? Tonnage { get; set; }

        [JsonProperty("InventorySlots", NullValueHandling = NullValueHandling.Ignore)]
        public long? InventorySlots { get; set; }

        [JsonProperty("MaxArmor", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxArmor { get; set; }

        [JsonProperty("MaxRearArmor", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxRearArmor { get; set; }

        [JsonProperty("InternalStructure", NullValueHandling = NullValueHandling.Ignore)]
        public long? InternalStructure { get; set; }
    }
    public partial class FixedEquipment
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

        [JsonProperty("IsFixed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsFixed { get; set; }
    }
    public partial class Hardpoint
    {
        [JsonProperty("WeaponMount", NullValueHandling = NullValueHandling.Ignore)]
        public WeaponMount? WeaponMount { get; set; }

        [JsonProperty("Omni", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Omni { get; set; }
    }
    public partial class LosPosition
    {
        [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
        public double? X { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public double? Y { get; set; }

        [JsonProperty("z", NullValueHandling = NullValueHandling.Ignore)]
        public double? Z { get; set; }
    }
    public partial class ChassisDef
    {
        public static ChassisDef FromJson(string json) => JsonConvert.DeserializeObject<ChassisDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }

}
