using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Utils.CodeGenerators
{
    public static class EnumGenerator
    {
        private const string enumTemplate = "public enum <EnumName>\r\n" +
                                            "{\r\n" +
                                            "<EnumValues>\r\n" +
                                            "}";

        public static string GenerateEnumFromStringValues(string enumName, List<string> values)
        {
            var enumDefinition = enumTemplate;
            var distinctValuesList = values.Distinct().ToList();

            enumDefinition = enumDefinition.Replace("<EnumName>", enumName);
            var enumValues = new List<string>(distinctValuesList.Count());

            foreach (var enumValue in distinctValuesList)
            {
                enumValues.Add($"[Description(\"{enumValue}\")] {enumValue.Replace(" ", "")},");
            }

            enumDefinition = enumDefinition.Replace("<EnumValues>", string.Join("\r\n", enumValues));

            return enumDefinition;
        }
    }
}