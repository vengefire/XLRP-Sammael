//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do one of these:
//
//    using BattleEngineJsonCreation;
//
//    var movementCapabilitiesDef = MovementCapabilitiesDef.FromJson(jsonString);
//    var hardpointDataDef = HardpointDataDef.FromJson(jsonString);
//    var chassisDef = ChassisDef.FromJson(jsonString);
//    var mechDef = MechDef.FromJson(jsonString);

namespace BattleEngineJsonCreation
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MovementCapabilitiesDef
    {
        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public MovementCapabilitiesDefDescription Description { get; set; }

        [JsonProperty("MaxWalkDistance", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxWalkDistance { get; set; }

        [JsonProperty("MaxSprintDistance", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxSprintDistance { get; set; }

        [JsonProperty("WalkVelocity", NullValueHandling = NullValueHandling.Ignore)]
        public long? WalkVelocity { get; set; }

        [JsonProperty("RunVelocity", NullValueHandling = NullValueHandling.Ignore)]
        public long? RunVelocity { get; set; }

        [JsonProperty("SprintVelocity", NullValueHandling = NullValueHandling.Ignore)]
        public long? SprintVelocity { get; set; }

        [JsonProperty("LimpVelocity", NullValueHandling = NullValueHandling.Ignore)]
        public long? LimpVelocity { get; set; }

        [JsonProperty("WalkAcceleration", NullValueHandling = NullValueHandling.Ignore)]
        public long? WalkAcceleration { get; set; }

        [JsonProperty("SprintAcceleration", NullValueHandling = NullValueHandling.Ignore)]
        public long? SprintAcceleration { get; set; }

        [JsonProperty("MaxRadialVelocity", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxRadialVelocity { get; set; }

        [JsonProperty("MaxRadialAcceleration", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxRadialAcceleration { get; set; }

        [JsonProperty("MaxJumpVel", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxJumpVel { get; set; }

        [JsonProperty("MaxJumpAccel", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxJumpAccel { get; set; }

        [JsonProperty("MaxJumpVerticalAccel", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxJumpVerticalAccel { get; set; }
    }

    public partial class MovementCapabilitiesDefDescription
    {
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        [JsonProperty("Icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }
    }

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
        public long? Tonnage { get; set; }

        [JsonProperty("InitialTonnage", NullValueHandling = NullValueHandling.Ignore)]
        public float? InitialTonnage { get; set; }

        [JsonProperty("weightClass", NullValueHandling = NullValueHandling.Ignore)]
        public string WeightClass { get; set; }

        [JsonProperty("BattleValue", NullValueHandling = NullValueHandling.Ignore)]
        public long? BattleValue { get; set; }

        [JsonProperty("Heatsinks", NullValueHandling = NullValueHandling.Ignore)]
        public long? Heatsinks { get; set; }

        [JsonProperty("TopSpeed", NullValueHandling = NullValueHandling.Ignore)]
        public long? TopSpeed { get; set; }

        [JsonProperty("TurnRadius", NullValueHandling = NullValueHandling.Ignore)]
        public long? TurnRadius { get; set; }

        [JsonProperty("MaxJumpjets", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxJumpjets { get; set; }

        [JsonProperty("Stability", NullValueHandling = NullValueHandling.Ignore)]
        public long? Stability { get; set; }

        [JsonProperty("StabilityDefenses", NullValueHandling = NullValueHandling.Ignore)]
        public List<long> StabilityDefenses { get; set; }

        [JsonProperty("SpotterDistanceMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public long? SpotterDistanceMultiplier { get; set; }

        [JsonProperty("VisibilityMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public long? VisibilityMultiplier { get; set; }

        [JsonProperty("SensorRangeMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public long? SensorRangeMultiplier { get; set; }

        [JsonProperty("Signature", NullValueHandling = NullValueHandling.Ignore)]
        public long? Signature { get; set; }

        [JsonProperty("Radius", NullValueHandling = NullValueHandling.Ignore)]
        public long? Radius { get; set; }

        [JsonProperty("PunchesWithLeftArm", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PunchesWithLeftArm { get; set; }

        [JsonProperty("MeleeDamage", NullValueHandling = NullValueHandling.Ignore)]
        public long? MeleeDamage { get; set; }

        [JsonProperty("MeleeInstability", NullValueHandling = NullValueHandling.Ignore)]
        public long? MeleeInstability { get; set; }

        [JsonProperty("MeleeToHitModifier", NullValueHandling = NullValueHandling.Ignore)]
        public long? MeleeToHitModifier { get; set; }

        [JsonProperty("DFADamage", NullValueHandling = NullValueHandling.Ignore)]
        public long? DfaDamage { get; set; }

        [JsonProperty("DFAToHitModifier", NullValueHandling = NullValueHandling.Ignore)]
        public long? DfaToHitModifier { get; set; }

        [JsonProperty("DFASelfDamage", NullValueHandling = NullValueHandling.Ignore)]
        public long? DfaSelfDamage { get; set; }

        [JsonProperty("DFAInstability", NullValueHandling = NullValueHandling.Ignore)]
        public long? DfaInstability { get; set; }

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

    public partial class Tags
    {
        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Items { get; set; }

        [JsonProperty("tagSetSourceFile", NullValueHandling = NullValueHandling.Ignore)]
        public string TagSetSourceFile { get; set; }
    }

    public partial class DefDescription
    {
        [JsonProperty("Cost", NullValueHandling = NullValueHandling.Ignore)]
        public long? Cost { get; set; }

        [JsonProperty("Rarity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Rarity { get; set; }

        [JsonProperty("Purchasable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Purchasable { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }

        [JsonProperty("UIName", NullValueHandling = NullValueHandling.Ignore)]
        public string UiName { get; set; }

        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        [JsonProperty("Icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }
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
        public List<FixedEquipment> Inventory { get; set; }

        [JsonProperty("MechTags", NullValueHandling = NullValueHandling.Ignore)]
        public Tags MechTags { get; set; }

        [JsonProperty("MinAppearanceDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? MinAppearanceDate { get; set; }
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

    public enum ComponentDefType { AmmunitionBox, HeatSink, JumpJet, Upgrade, Weapon };

    public enum DamageLevel { Functional };

    public enum Location { CenterTorso, Head, LeftArm, LeftLeg, LeftTorso, RightArm, RightLeg, RightTorso };

    public enum WeaponMount { AntiPersonnel, Ballistic, Energy, Missile };

    public partial class MovementCapabilitiesDef
    {
        public static MovementCapabilitiesDef FromJson(string json) => JsonConvert.DeserializeObject<MovementCapabilitiesDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }

    public partial class HardpointDataDef
    {
        public static HardpointDataDef FromJson(string json) => JsonConvert.DeserializeObject<HardpointDataDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }

    public partial class ChassisDef
    {
        public static ChassisDef FromJson(string json) => JsonConvert.DeserializeObject<ChassisDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }

    public partial class MechDef
    {
        public static MechDef FromJson(string json) => JsonConvert.DeserializeObject<MechDef>(json, BattleEngineJsonCreation.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MovementCapabilitiesDef self) => JsonConvert.SerializeObject(self, BattleEngineJsonCreation.Converter.Settings);
        public static string ToJson(this HardpointDataDef self) => JsonConvert.SerializeObject(self, BattleEngineJsonCreation.Converter.Settings);
        public static string ToJson(this ChassisDef self) => JsonConvert.SerializeObject(self, BattleEngineJsonCreation.Converter.Settings);
        public static string ToJson(this MechDef self) => JsonConvert.SerializeObject(self, BattleEngineJsonCreation.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ComponentDefTypeConverter.Singleton,
                DamageLevelConverter.Singleton,
                LocationConverter.Singleton,
                WeaponMountConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ComponentDefTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ComponentDefType) || t == typeof(ComponentDefType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AmmunitionBox":
                    return ComponentDefType.AmmunitionBox;
                case "HeatSink":
                    return ComponentDefType.HeatSink;
                case "JumpJet":
                    return ComponentDefType.JumpJet;
                case "Upgrade":
                    return ComponentDefType.Upgrade;
                case "Weapon":
                    return ComponentDefType.Weapon;
            }
            throw new Exception("Cannot unmarshal type ComponentDefType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ComponentDefType)untypedValue;
            switch (value)
            {
                case ComponentDefType.AmmunitionBox:
                    serializer.Serialize(writer, "AmmunitionBox");
                    return;
                case ComponentDefType.HeatSink:
                    serializer.Serialize(writer, "HeatSink");
                    return;
                case ComponentDefType.JumpJet:
                    serializer.Serialize(writer, "JumpJet");
                    return;
                case ComponentDefType.Upgrade:
                    serializer.Serialize(writer, "Upgrade");
                    return;
                case ComponentDefType.Weapon:
                    serializer.Serialize(writer, "Weapon");
                    return;
            }
            throw new Exception("Cannot marshal type ComponentDefType");
        }

        public static readonly ComponentDefTypeConverter Singleton = new ComponentDefTypeConverter();
    }

    internal class DamageLevelConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DamageLevel) || t == typeof(DamageLevel?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Functional")
            {
                return DamageLevel.Functional;
            }
            throw new Exception("Cannot unmarshal type DamageLevel");
        }
        
        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (DamageLevel)untypedValue;
            if (value == DamageLevel.Functional)
            {
                serializer.Serialize(writer, "Functional");
                return;
            }
            throw new Exception("Cannot marshal type DamageLevel");
        }

        public static readonly DamageLevelConverter Singleton = new DamageLevelConverter();
    }

    internal class LocationConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Location) || t == typeof(Location?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "CenterTorso":
                    return Location.CenterTorso;
                case "Head":
                    return Location.Head;
                case "LeftArm":
                    return Location.LeftArm;
                case "LeftLeg":
                    return Location.LeftLeg;
                case "LeftTorso":
                    return Location.LeftTorso;
                case "RightArm":
                    return Location.RightArm;
                case "RightLeg":
                    return Location.RightLeg;
                case "RightTorso":
                    return Location.RightTorso;
            }
            throw new Exception("Cannot unmarshal type Location");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Location)untypedValue;
            switch (value)
            {
                case Location.CenterTorso:
                    serializer.Serialize(writer, "CenterTorso");
                    return;
                case Location.Head:
                    serializer.Serialize(writer, "Head");
                    return;
                case Location.LeftArm:
                    serializer.Serialize(writer, "LeftArm");
                    return;
                case Location.LeftLeg:
                    serializer.Serialize(writer, "LeftLeg");
                    return;
                case Location.LeftTorso:
                    serializer.Serialize(writer, "LeftTorso");
                    return;
                case Location.RightArm:
                    serializer.Serialize(writer, "RightArm");
                    return;
                case Location.RightLeg:
                    serializer.Serialize(writer, "RightLeg");
                    return;
                case Location.RightTorso:
                    serializer.Serialize(writer, "RightTorso");
                    return;
            }
            throw new Exception("Cannot marshal type Location");
        }

        public static readonly LocationConverter Singleton = new LocationConverter();
    }
    internal class WeaponMountConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(WeaponMount) || t == typeof(WeaponMount?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AntiPersonnel":
                    return WeaponMount.AntiPersonnel;
                case "Ballistic":
                    return WeaponMount.Ballistic;
                case "Energy":
                    return WeaponMount.Energy;
                case "Missile":
                    return WeaponMount.Missile;
            }
            throw new Exception("Cannot unmarshal type WeaponMount");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (WeaponMount)untypedValue;
            switch (value)
            {
                case WeaponMount.AntiPersonnel:
                    serializer.Serialize(writer, "AntiPersonnel");
                    return;
                case WeaponMount.Ballistic:
                    serializer.Serialize(writer, "Ballistic");
                    return;
                case WeaponMount.Energy:
                    serializer.Serialize(writer, "Energy");
                    return;
                case WeaponMount.Missile:
                    serializer.Serialize(writer, "Missile");
                    return;
            }
            throw new Exception("Cannot marshal type WeaponMount");
        }

        public static readonly WeaponMountConverter Singleton = new WeaponMountConverter();
    }
}
