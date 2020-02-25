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
            var settings = settingsLoad.LoadSettings();
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
            var cabDictionary = Reuse.PreFabDicPop(Directory.GetFiles(Path.Combine(settings.BtInstallDir, "mods\\CommunityAssets"), "chrprfmech*", SearchOption.AllDirectories));
            var btDictionary = Reuse.PreFabDicPop(Directory.GetFiles(Path.Combine(settings.BtInstallDir, "BattleTech_Data\\StreamingAssets\\data\\assetbundles"), "chrprfmech*", SearchOption.AllDirectories));
            //(Count should be 177)
            var preFabDictionary = dlcDictionary.Concat(cabDictionary).Concat(btDictionary).ToDictionary(k => k.Key, k => k.Value);
            //Populate Gear Dictionary Tuple (key, vaule, componentDefType)
            var gearDic = ComponetDictionaryParse.ComDefDicPop(Directory.GetFiles(settings.BtInstallDir, "*.json", SearchOption.AllDirectories));
            string[] chassisFiles = Directory.GetFiles(settings.ChassisDefpath, "chassis*.json", SearchOption.AllDirectories);
            string[] chassisNames = null;
            foreach (var file in Directory.GetFiles(settings.BedPath, "*.bed", SearchOption.AllDirectories))
            {
                string[] filelines = System.IO.File.ReadAllLines(file);
                if (filelines[3].Contains("Biped"))
                {
                    if (filelines[0].Contains("Annihilator"))
                    {
                        string something = "break";
                    }
                    var cabCheck = new List<string>(Reuse.CabCheck(filelines[0]));
                    for (int i = 0; i < cabCheck.Count; i++)
                    {
                        if (preFabDictionary.ContainsKey(cabCheck[i]))
                        {
                            chassisNames = MechBuilder.ChassisName(file);
                            var chassisDef = MechBuilder.ChassisDefs(chassisNames, chassisFiles, false);
                            if (chassisDef.Description != null)
                            {
                                Console.WriteLine(file + " " + chassisDef.Description.Id);
                                var mechDef = MechBuilder.MechDefs(chassisDef, file);
                                mechDef = MechBuilder.MechLocations(gearDic, mechDef, file);
                                mechDef = MechBuilder.Engines(chassisDef, mechDef, file);
                                string outputmechDef = Newtonsoft.Json.JsonConvert.SerializeObject(mechDef, Newtonsoft.Json.Formatting.Indented, BattleEngineJsonCreation.Converter.Settings);
                                File.WriteAllText(Path.Combine(settings.OutputDir, mechDef.Description.Id + ".json"), outputmechDef);
                                //var mechDef = MechBuilder.MechDefs(chassisDef);
                                break;
                            }
                        }
                    }
                }
                if (chassisNames.Length == 0) Log(Path.GetFileName(file) + ",NO CAB", settings.OutputDir);
            }
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
