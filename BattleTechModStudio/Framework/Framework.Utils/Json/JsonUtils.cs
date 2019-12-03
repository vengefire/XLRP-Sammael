using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonUtils
{
    public static string JsonPrettify(string json)
    {
        using (var stringReader = new StringReader(json))
        {
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) {Formatting = Formatting.Indented};
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }

    public static JObject DeserializeJson(string filePath)
    {
        var rgx = new Regex(@"(\]|\}|""|[A-Za-z0-9])\s*\n\s*(\[|\{|"")", RegexOptions.Singleline);
        var commasAdded = rgx.Replace(File.ReadAllText(filePath), "$1,\n$2");
        var jsonObject = (JObject) JsonConvert.DeserializeObject(commasAdded);
        return jsonObject;
    }
}