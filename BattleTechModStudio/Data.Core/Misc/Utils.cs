namespace Data.Core.Misc
{
    public static class Utils
    {
        public static string GetPrefabIdFromPath(string path)
        {
            return path.Substring(path.LastIndexOf('/') + 1).Replace(".prefab", "");
        }
    }
}