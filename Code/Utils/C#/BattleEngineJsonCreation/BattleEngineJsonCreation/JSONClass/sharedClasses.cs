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

    public enum ComponentDefType { AmmunitionBox, HeatSink, JumpJet, Upgrade, Weapon };

    public enum DamageLevel { Functional };

    public enum Location { CenterTorso, Head, LeftArm, LeftLeg, LeftTorso, RightArm, RightLeg, RightTorso };

    public enum WeaponMount { AntiPersonnel, Ballistic, Energy, Missile };
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
