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
            var dlcDictionary = new Dictionary<string, string> { };
            var cabDictionary = new Dictionary<string, string> { };
            var btDictionary= new Dictionary<string, string> { };
            var preFabDictionary= new Dictionary<string, string> { };
            var gearDic = new Dictionary<string, (string, ComponentDefType)> { };

            if (settings.UpdateCSV == true)
            {
                dlcDictionary = new Dictionary<string, string>             {
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
                cabDictionary = Reuse.PreFabDicPop(Directory.GetFiles(Path.Combine(settings.BtInstallDir, "mods\\CommunityAssets"), "chrprfmech*", SearchOption.AllDirectories));
                btDictionary = Reuse.PreFabDicPop(Directory.GetFiles(Path.Combine(settings.BtInstallDir, "BattleTech_Data\\StreamingAssets\\data\\assetbundles"), "chrprfmech*", SearchOption.AllDirectories));
                //(Count should be 177)
                preFabDictionary = dlcDictionary.Concat(cabDictionary).Concat(btDictionary).ToDictionary(k => k.Key, k => k.Value);
                //Populate Gear Dictionary Tuple (key, vaule, componentDefType)
                gearDic = ComponetDictionaryParse.ComDefDicPop(Directory.GetFiles(settings.BtInstallDir, "*.json", SearchOption.AllDirectories));
                String csv = String.Join(Environment.NewLine, preFabDictionary.Select(d => $"{d.Key},{d.Value}"));
                csv = csv.Replace("(", "");
                csv = csv.Replace(")", "");
                String csv2 = String.Join(Environment.NewLine, gearDic.Select(d => $"{d.Key},{d.Value}"));
                csv2 = csv2.Replace("(", "");
                csv2 = csv2.Replace(")", "");
                File.WriteAllText(Path.Combine(settings.OutputDir, "preFabDictionary.csv"), csv);
                File.WriteAllText(Path.Combine(settings.OutputDir, "gearDic.csv"), csv2);
                Reuse.EndProgram("CSV files have been updated, pleaes change the settings.json to UpdateCSV=false");
            }
            else
            {
                preFabDictionary = File.ReadLines("preFabDictionary.csv").Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
                //gearDic = File.ReadLines("preFabDictionary.csv").Select(line => line.Split('|')).ToDictionary(line => line[0], line => line[1]);
                //gearDic = ComponetDictionaryParse.ComDefDicPop(Directory.GetFiles(settings.BtInstallDir, "*.json", SearchOption.AllDirectories));
                gearDic = File.ReadLines("gearDic.csv").Select(line => line.Split(',')).ToDictionary(line => line[0], line => (line[1], (ComponentDefType)Enum.Parse(typeof(ComponentDefType), line[2])));
            }

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
                        string outputmechDef = Newtonsoft.Json.JsonConvert.SerializeObject(mechDef, Newtonsoft.Json.Formatting.Indented, Converter.Settings);
                        File.WriteAllText(Path.Combine(settings.OutputDir, mechDef.Description.Id + ".json"), outputmechDef);
                        logMemory = logMemory + Path.GetFileName(file) + "," + mechDef.Description.Id + ",Generated\r\n";
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
            File.WriteAllText(logdir + "output.csv", logMessage);
        }
    }
}
