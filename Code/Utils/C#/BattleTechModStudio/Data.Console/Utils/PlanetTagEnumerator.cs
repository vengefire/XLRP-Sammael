using System.Collections.Generic;
using System.Linq;
using Data.Core.Enums;
using Data.Core.GameObjects;
using Newtonsoft.Json.Linq;

namespace Data.Console.Utils
{
    internal static class PlanetTagEnumerator
    {
        public class PlanetTag
        {
            public string category { get; }
            public string value { get; }

            public PlanetTag(string category, string value)
            {
                this.category = category;
                this.value = value;
            }
        }

        public static List<PlanetTag> EnumeratePlanetTags(List<ManifestEntry> manifestEntries)
        {
            var planetTags = new HashSet<string>();
            manifestEntries.Where(entry => entry.GameObjectType == GameObjectTypeEnum.StarSystemDef)
                .ToList()
                .ForEach(entry =>
                {
                    var tags = (JArray) entry.Json["Tags"]?["items"];
                    tags?.Where(token => !token.Value<string>().Contains("planet_name_")).ToList().ForEach(token => planetTags.Add(token.Value<string>()));
                });

            var sortedList = planetTags.ToList();
            var parsedList = sortedList.Select(s =>
            {
                var parts = s.Split('_');
                return new PlanetTag(parts[1], parts[2]);
            });

            return parsedList.ToList();

            // return new List<dynamic> {parsedList.ToList()};
            // var groupedList = parsedList.GroupBy(arg => arg.categogry, (category, tags) => tags).ToList();
            // sortedList.Sort(string.CompareOrdinal);
            // return sortedList;
        }
    }
}