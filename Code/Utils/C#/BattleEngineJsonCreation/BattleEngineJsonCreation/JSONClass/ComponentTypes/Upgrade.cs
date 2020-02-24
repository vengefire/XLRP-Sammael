﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using BattleEngineJsonCreation;
//
//    var upgrade = Upgrade.FromJson(jsonString);

namespace BattleEngineJsonCreation
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Upgrade
    {

        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public UpgradeDescription Description { get; set; }

        [JsonProperty("BonusValueA", NullValueHandling = NullValueHandling.Ignore)]
        public string BonusValueA { get; set; }

        [JsonProperty("BonusValueB", NullValueHandling = NullValueHandling.Ignore)]
        public string BonusValueB { get; set; }

        [JsonProperty("ComponentType", NullValueHandling = NullValueHandling.Ignore)]
        public string ComponentType { get; set; }

        [JsonProperty("ComponentSubType", NullValueHandling = NullValueHandling.Ignore)]
        public string ComponentSubType { get; set; }

        [JsonProperty("PrefabIdentifier", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefabIdentifier { get; set; }

        [JsonProperty("BattleValue", NullValueHandling = NullValueHandling.Ignore)]
        public long? BattleValue { get; set; }

        [JsonProperty("InventorySize", NullValueHandling = NullValueHandling.Ignore)]
        public long? InventorySize { get; set; }

        [JsonProperty("Tonnage", NullValueHandling = NullValueHandling.Ignore)]
        public long? Tonnage { get; set; }

        [JsonProperty("AllowedLocations", NullValueHandling = NullValueHandling.Ignore)]
        public string AllowedLocations { get; set; }

        [JsonProperty("DisallowedLocations", NullValueHandling = NullValueHandling.Ignore)]
        public string DisallowedLocations { get; set; }

        [JsonProperty("CriticalComponent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CriticalComponent { get; set; }

        [JsonProperty("statusEffects")]
        public List<StatusEffect> StatusEffects { get; set; }

        [JsonProperty("ComponentTags", NullValueHandling = NullValueHandling.Ignore)]
        public ComponentTags ComponentTags { get; set; }

        [JsonProperty("StatName")]
        public object StatName { get; set; }

        [JsonProperty("RelativeModifier", NullValueHandling = NullValueHandling.Ignore)]
        public long? RelativeModifier { get; set; }

        [JsonProperty("AbsoluteModifier", NullValueHandling = NullValueHandling.Ignore)]
        public long? AbsoluteModifier { get; set; }
    }

    public partial class Custom
    {

        [JsonProperty("ArmActuator", NullValueHandling = NullValueHandling.Ignore)]
        public ArmActuator ArmActuator { get; set; }

        [JsonProperty("DynamicSlots", NullValueHandling = NullValueHandling.Ignore)]
        public DynamicSlots DynamicSlots { get; set; }

        [JsonProperty("Weights", NullValueHandling = NullValueHandling.Ignore)]
        public Weights Weights { get; set; }

        [JsonProperty("CASE", NullValueHandling = NullValueHandling.Ignore)]
        public Case Case { get; set; }
    }

    public partial class ArmActuator
    {
        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }

    public partial class Case
    {
        [JsonProperty("AllLocations", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllLocations { get; set; }
    }

    public partial class DynamicSlots
    {
        [JsonProperty("ReservedSlots", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReservedSlots { get; set; }
    }

    public partial class Weights
    {
        [JsonProperty("ArmorFactor", NullValueHandling = NullValueHandling.Ignore)]
        public double? ArmorFactor { get; set; }
    }

    public partial class UpgradeDescription
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

    public partial class StatusEffect
    {
        [JsonProperty("durationData", NullValueHandling = NullValueHandling.Ignore)]
        public DurationData DurationData { get; set; }

        [JsonProperty("targetingData", NullValueHandling = NullValueHandling.Ignore)]
        public TargetingData TargetingData { get; set; }

        [JsonProperty("effectType", NullValueHandling = NullValueHandling.Ignore)]
        public string EffectType { get; set; }

        [JsonProperty("Description", NullValueHandling = NullValueHandling.Ignore)]
        public StatusEffectDescription Description { get; set; }

        [JsonProperty("nature", NullValueHandling = NullValueHandling.Ignore)]
        public string Nature { get; set; }

        [JsonProperty("statisticData", NullValueHandling = NullValueHandling.Ignore)]
        public StatisticData StatisticData { get; set; }
    }

    public partial class StatusEffectDescription
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

    public partial class DurationData
    {
    }

    public partial class StatisticData
    {
        [JsonProperty("statName", NullValueHandling = NullValueHandling.Ignore)]
        public string StatName { get; set; }

        [JsonProperty("operation", NullValueHandling = NullValueHandling.Ignore)]
        public string Operation { get; set; }

        [JsonProperty("modValue", NullValueHandling = NullValueHandling.Ignore)]
        public string ModValue { get; set; }

        [JsonProperty("modType", NullValueHandling = NullValueHandling.Ignore)]
        public string ModType { get; set; }

        [JsonProperty("targetCollection", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetCollection { get; set; }

        [JsonProperty("targetWeaponSubType", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetWeaponSubType { get; set; }
    }

    public partial class TargetingData
    {
        [JsonProperty("effectTriggerType", NullValueHandling = NullValueHandling.Ignore)]
        public string EffectTriggerType { get; set; }

        [JsonProperty("effectTargetType", NullValueHandling = NullValueHandling.Ignore)]
        public string EffectTargetType { get; set; }

        [JsonProperty("showInStatusPanel", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowInStatusPanel { get; set; }
    }

    public partial class Upgrade
    {
        public static Upgrade FromJson(string json) => JsonConvert.DeserializeObject<Upgrade>(json, BattleEngineJsonCreation.Converter.Settings);
    }
}

