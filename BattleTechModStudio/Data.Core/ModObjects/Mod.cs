using System.Collections.Generic;

namespace Data.Core.ModObjects
{
    public class Mod : ModBase
    {
        public bool IsValid = false;

        public Mod(ModBase modBase)
        {
            InitFromBase(modBase);
        }

        public List<Mod> DependsOn { get; set; } = new List<Mod>();
        public List<Mod> ConflictsWith { get; set; } = new List<Mod>();
        public List<Mod> OptionallyDependsOn { get; set; } = new List<Mod>();
        public List<string> InvalidReasonList { get; } = new List<string>();
        public int LoadOrder { get; set; }

        public int LoadCycle { get; set; }
    }
}