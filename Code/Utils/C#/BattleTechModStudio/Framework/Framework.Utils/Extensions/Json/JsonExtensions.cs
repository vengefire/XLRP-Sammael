using Newtonsoft.Json.Linq;

namespace Framework.Utils.Extensions.Json
{
    public static class JsonExtensions
    {
        public static bool IsNullOrEmpty(this JToken token)
        {
            return token == null || token.Type == JTokenType.Array && !token.HasValues
                                 || token.Type == JTokenType.Object && !token.HasValues
                                 || token.Type == JTokenType.String && token.ToString() == string.Empty
                                 || token.Type == JTokenType.Null;
        }

        public static bool IsNullOrEmpty(this JValue token)
        {
            return token == null || token.Type == JTokenType.Array && !token.HasValues
                                 || token.Type == JTokenType.Object && !token.HasValues
                                 || token.Type == JTokenType.String && token.ToString() == string.Empty
                                 || token.Type == JTokenType.Null;
        }
    }
}