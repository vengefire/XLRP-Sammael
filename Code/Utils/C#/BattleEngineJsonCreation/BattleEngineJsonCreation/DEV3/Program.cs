using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    class Program
    {
        static void Main()
        {
            string logMemory = "";
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
            string[] bedfiles = Directory.GetFiles(settings.BedPath, "*.bed", SearchOption.AllDirectories);
            Parallel.ForEach(bedfiles, (file) =>
            //foreach (var file in Directory.GetFiles(settings.BedPath, "*.bed", SearchOption.AllDirectories))
            {
                Console.WriteLine($"Processing {file} on thread {Thread.CurrentThread.ManagedThreadId}");
                bool cabCheck = MechBuilder.CabCheck(preFabDictionary, file);
                if (cabCheck == true)
                {
                    chassisNames = MechBuilder.ChassisName(file);
                    var chassisDef = MechBuilder.ChassisDefs(chassisNames, chassisFiles, false);
                    if (chassisDef.Description != null)
                    {
                        //Console.WriteLine(file + " " + chassisDef.Description.Id);
                        var mechDef = MechBuilder.MechDefs(chassisDef, file);
                        mechDef = MechBuilder.MechLocations(gearDic, mechDef, file);
                        mechDef = MechBuilder.Engines(chassisDef, mechDef, file);
                        string outputmechDef = Newtonsoft.Json.JsonConvert.SerializeObject(mechDef, Newtonsoft.Json.Formatting.Indented, BattleEngineJsonCreation.Converter.Settings);
                        File.WriteAllText(Path.Combine(settings.OutputDir, mechDef.Description.Id + ".json"), outputmechDef);
                        logMemory = logMemory + Path.GetFileName(file) + "," + mechDef.Description.Id+ ",Generated\r\n";
                        //var mechDef = MechBuilder.MechDefs(chassisDef);
                        //break;
                    }
                    else
                    {
                        logMemory = logMemory + Path.GetFileName(file) + ",NO ChassisDef File,Not Generated\r\n";
                    }
                }
                else
                {
                    logMemory = logMemory + Path.GetFileName(file) + ",NO CAB,Not Generated\r\n";
                }
                });
            //}
            Log(logMemory, settings.OutputDir);
        }
        public static void Log(string logMessage, string logdir)
        {
            File.WriteAllText(logdir + "output.csv",logMessage);
        }
    }
}
