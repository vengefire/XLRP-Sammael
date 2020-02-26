using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleEngineJsonCreation
{
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
    public partial class MovementCapabilitiesDef
    {
        public static MovementCapabilitiesDef FromJson(string json) => JsonConvert.DeserializeObject<MovementCapabilitiesDef>(json, Converter.Settings);
    }
}
