using System.Linq;

namespace Data.Core.GameObjects
{
    public class JsonObjectBase
    {
        public void InitFromBase<TBase>(TBase baseObject)
        {
            //Get the list of properties available in base class
            var baseProperties = baseObject.GetType().GetProperties();

            baseProperties.ToList().ForEach(property =>
            {
                //Check whether that property is present in derived class
                var isPresent = GetType().GetProperty(property.Name);
                if (isPresent != null && property.CanWrite)
                {
                    //If present get the value and map it
                    var value = baseObject.GetType().GetProperty(property.Name)?.GetValue(baseObject, null);
                    GetType().GetProperty(property.Name)?.SetValue(this, value, null);
                }
            });
        }
    }
}