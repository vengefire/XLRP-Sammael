using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load Settings.json and int
            var settings = settingsLoad.LoadSettings();
            //Hidden DLC Prefabs Added by hand
            var dlcDictionary = new Dictionary<string, string>             {
                //DLCs these files are hidden in assest and have to be added by hand
                {"crab","chrprfmech_crabbase-001"},
                {"cyclops","chrprfmech_cyclopsbase-001"},
                {"hatchetman","chrprfmech_hatchetmanbase-001"},
                {"annihilator","chrprfmech_annihilatorbase-001"},
                {"archer","chrprfmech_archerbase-001"},
                {"assassin","chrprfmech_assassinbase-001"},
                {"bullshark","chrprfmech_bullsharkbase-001"},
                {"flea","chrprfmech_fleabase-001"},
                {"phoenixhawk","chrprfmech_phoenixhawkbase-001"},
                {"rifleman","chrprfmech_riflemanbase-001"},
                {"vulcan","chrprfmech_vulcanbase-001"},
                {"javelin","chrprfmech_javelinmanbase-001"},
                {"raven","chrprfmech_ravenbase-001"},
            };
            //Calls Reuse Dictionary Population for CAB Directory
            var cabDictionary = Reuse.PreFabDicPop(Directory.GetFiles(Path.Combine(settings.BtInstallDir, "mods\\CommunityAssets"), "chrprfmech*", SearchOption.AllDirectories));
            //Calls Dictionary Population for BT Base install (Directory from Settings)
            var btDictionary = Reuse.PreFabDicPop(Directory.GetFiles(Path.Combine(settings.BtInstallDir, "BattleTech_Data\\StreamingAssets\\data\\assetbundles"), "chrprfmech*", SearchOption.AllDirectories));
            //Combine all preFab Dictionaries (Count should be 177)
            var preFabDictionary = dlcDictionary.Concat(cabDictionary).Concat(btDictionary).ToDictionary(k => k.Key, k => k.Value);
            //Load ChassisFiles
            string[] chassisFiles = Directory.GetFiles(settings.BtInstallDir, "chassis*.json", SearchOption.AllDirectories);
            //Load Bed files into array and parse
            string[] chassisNames = null;
            foreach (var file in Directory.GetFiles(settings.BedPath, "*.bed", SearchOption.AllDirectories))
            {
                string[] filelines = System.IO.File.ReadAllLines(file);
                var cabCheck = new List<string>(Reuse.CabCheck(filelines[0]));
                for (int i = 0; i < cabCheck.Count; i++)
                {
                    if (preFabDictionary.ContainsKey(cabCheck[i]))
                    {
                        //String Name, Variant, Shortname
                        chassisNames = chassisHelper.ChassisName(file);
                        //Return JSON chassisDefMatching Name
                        var chassisDef = chassisHelper.ChassisDefs(chassisNames, chassisFiles);
                        var mechDef = chassisHelper.MechDefs(chassisDef, file);
                        //Log(Path.GetFileName(file) + "," + chassisNames[0] + "," + chassisNames[1] + "," + chassisNames[2], settings.OutputDir);
                        break;
                    }
                }
                if (chassisNames.Length == 0) Log(Path.GetFileName(file) + ",NO CAB", settings.OutputDir);
            }

            //Populate Gear Dictionary Tuple (key, vaule, componentDefType)
            //var gearDic = ComponetDictionaryParse.ComDefDicPop(Directory.GetFiles(settings.BtInstallDir, "*.json", SearchOption.AllDirectories));

            // String csv2 = String.Join(Environment.NewLine, gearDic.Select(d => $"{d.Key},{d.Value}"));
            // System.IO.File.WriteAllText(Path.Combine(settings.OutputDir, "componentDefTuple.csv"), csv2);

            //Console.ReadKey();
        }
        public static void Log(string logMessage, string logdir)
        {
            using (StreamWriter w = File.AppendText(logdir + "log.txt"))
            {
                w.WriteLine(logMessage);
            }
        }
    }
}
