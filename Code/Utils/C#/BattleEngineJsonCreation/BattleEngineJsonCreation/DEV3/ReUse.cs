using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    public static class Reuse
    {
        public static void EndProgram(string reason)
        {
            Console.WriteLine(reason);
            Console.ReadKey();
            Environment.Exit(0);
        }
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c == ' ') || (c == ',') || (c == '-'))
                {
                    sb.Append(c);
                }
            }
            //Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
        public static Dictionary<string, string> PreFabDicPop(string[] files)
        {
            var dic = new Dictionary<string, string> { };
            try
            {
                foreach (string prefabs in files)
                {
                    string filename = Path.GetFileName(prefabs);
                    string mechname = filename.Replace("chrprfmech_", "").Replace("base-001", "");
                    dic.Add(mechname, filename);
                }
            }
            catch (Exception e)
            {
                Reuse.EndProgram(e.ToString());
            }
            return dic;
        }
        public static List<string> CabCheck(string cc)
        {
            var cabCheck = new List<string>(cc.Split(','));
            var newArray = cabCheck.ToArray();
            newArray = newArray[1].Split(' ');
            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = Reuse.RemoveSpecialCharacters(newArray[i].ToLower());
            }
            return newArray.ToList();
        }
    }
}
