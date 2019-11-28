using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Core.ModObjects;

namespace Data.Console.Utils
{
    internal static class TypeEnumGenerator
    {
        public static IEnumerable<string> GetUniqueTypes(List<ManifestEntry> manifestEntries)
        {
            return manifestEntries.Select(entry => entry.Type).ToList().Distinct().OrderBy(s => s);
        }

        public static string GenerateEnum(List<string> members, string nameSpace, string enumName)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"enum {enumName}");
            builder.AppendLine(@"{");
            members.ForEach(s => builder.AppendLine($"    {s},"));
            builder.AppendLine(@"}");
            return builder.ToString();
        }
    }
}