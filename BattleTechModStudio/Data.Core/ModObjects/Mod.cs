using System.Collections.Generic;
using System.Linq;

namespace Data.Core.ModObjects
{
    public class Mod : ModBase
    {
        public Mod(ModBase modBase)
        {
            //Get the list of properties available in base class
            var baseProperties = modBase.GetType().GetProperties();

            baseProperties.ToList().ForEach(property =>
            {
                //Check whether that property is present in derived class
                var isPresent = this.GetType().GetProperty(property.Name);
                if (isPresent != null && property.CanWrite)
                {
                    //If present get the value and map it
                    var value = modBase.GetType().GetProperty(property.Name)?.GetValue(modBase, null);
                    this.GetType().GetProperty(property.Name)?.SetValue(this, value, null);
                }
            });
        }

        public bool IsValid = false;
        public List<Mod> DependsOn { get; set; } = new List<Mod>();
        public List<Mod> ConflictsWith { get; set; } = new List<Mod>();
        public List<Mod> OptionallyDependsOn { get; set; } = new List<Mod>();
        public List<string> InvalidReasonList { get; } = new List<string>();
        public int LoadOrder { get; set; }
    }
}