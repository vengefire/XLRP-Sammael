using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Data.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static T FromEnumStringValue<T>(this string description) where T : struct
        {
            Debug.Assert(typeof(T).IsEnum);

            return (T) typeof(T)
                .GetFields()
                .First(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .Cast<DescriptionAttribute>()
                    .Any(a => a.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                )
                .GetValue(null);
        }

        public static string StringValue(this Enum enumItem)
        {
            return enumItem
                .GetType()
                .GetField(enumItem.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .Select(a => a.Description)
                .FirstOrDefault() ?? enumItem.ToString();
        }
    }
}