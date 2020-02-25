using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace BattleEngineJsonCreation
{
    static class BattleEngineConverter
    {

        static void Main()
        {
            string filepath = Directory.GetCurrentDirectory();
            //Settings Processing and Loading
            string settingfile = Path.Combine(filepath, "settings.json");
            string[] bedfiles = null;
            string[] chassisDefFiles = null;
            //string[] mechDefs = null;
            string cabDirectory = null;
            string btinstall = null;
            bool useMods = false;
            bool generateFromChassis = false;
            string outputDir = filepath;
            var settings = new Settings();
            if (File.Exists(Path.Combine(filepath, settingfile)))
            {
                string jsonString = File.ReadAllText(settingfile);
                settings = Settings.FromJson(jsonString);
                if (Directory.Exists(settings.BedPath))
                {
                    bedfiles = Directory.GetFiles(settings.BedPath, "*.bed", SearchOption.AllDirectories);
                }
                else
                {
                    EndProgram(settings.BedPath + " directory not found.");
                }
                generateFromChassis = settings.GenerateFromchassisDef;
                if (generateFromChassis == true)
                {
                    if (Directory.Exists(settings.ChassisDefpath))
                    {
                        chassisDefFiles = Directory.GetFiles(settings.ChassisDefpath, "chassisDef*.json");
                        for (int i = 0; i < chassisDefFiles.Length; i++)
                        {
                            chassisDefFiles[i] = Path.GetFileName(chassisDefFiles[i]);
                        }
                    }
                    else
                    {
                        EndProgram("Generate from Chassis set to true, but chassis directory not found");
                    }
                }
                //mechDefs = Directory.GetFiles(settings.MechDefPath, "mechDef*.json");
                btinstall = settings.BtInstallDir;
                if (Directory.Exists(Path.Combine(btinstall, "mods\\CommunityAssets")))
                {
                    cabDirectory = Path.Combine(btinstall, "mods\\CommunityAssets");
                }
                else
                {
                    EndProgram("Cab not installed in Mods Directory [CAB IS REQUIRED]");
                }
                useMods = settings.LoadDefsFromMods;
                outputDir = settings.OutputDir;
            }
            else
            {
                EndProgram("Settings File not found.");
            }
            if (bedfiles.Length == 0)
            {
                EndProgram("No Bed Files Found");
            }
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(Path.Combine(filepath, outputDir));
            }
            //Build preFab Files
            string[] cabpreFabs = Directory.GetFiles(cabDirectory, "chrprfmech*", SearchOption.AllDirectories);
            string btpreFabsDir = Path.Combine(btinstall, "BattleTech_Data\\StreamingAssets\\data\\assetbundles");
            string[] btpreFabs = Directory.GetFiles(btpreFabsDir, "chrprfmech*", SearchOption.AllDirectories);
            var preFabDictionary = new Dictionary<string, string>
            {
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
            foreach (string prefabs in cabpreFabs)
            {
                string filename = Path.GetFileName(prefabs);
                string mechname = filename.Replace("chrprfmech_", "").Replace("base-001", "");
                preFabDictionary.Add(mechname, filename);
            }
            foreach (string prefabs in btpreFabs)
            {
                string filename = Path.GetFileName(prefabs);
                string mechname = filename.Replace("chrprfmech_", "").Replace("base-001", "");
                preFabDictionary.Add(mechname, filename);
            }
            //Build componentDefs
            string[] jsonFiles = Directory.GetFiles(btinstall, "*.json", SearchOption.AllDirectories);
            var componentDefDictionaryTuple = new Dictionary<string, (string, ComponentDefType)>();
            /*{
            Weapons Laser
            //{ "Large Laser", ("Weapon_Laser_LargeLaser_0-STOCK",ComponentDefType.Weapon)},
            //{ "Medium Laser", ("Weapon_Laser_MediumLaser_0-STOCK",ComponentDefType.Weapon)},
            //{ "Small Laser", ("Weapon_Laser_SmallLaser_0-STOCK",ComponentDefType.Weapon)},
            //Pulse
            { "Large Pulse Laser", ("Weapon_Laser_LargeLaserPulse_0-STOCK",ComponentDefType.Weapon)},
            { "Medium Pulse Laser", ("Weapon_Laser_MediumLaserPulse_0-STOCK",ComponentDefType.Weapon)},
            { "Small Pulse Laser", ("Weapon_Laser_SmallLaserPulse_0-STOCK",ComponentDefType.Weapon)},
            //ER
            { "ER Large Laser", ("Weapon_Laser_LargeLaserER_0-STOCK",ComponentDefType.Weapon)},
            { "ER Medium Laser", ("Weapon_Laser_MediumLaserER_0-STOCK",ComponentDefType.Weapon)},
            { "ER Small Laser", ("Weapon_Laser_SmallLaserER_0-STOCK",ComponentDefType.Weapon)},
            //PPC
            { "PPC", ("Weapon_PPC_PPC_0-STOCK",ComponentDefType.Weapon)},
            { "ER PPC", ("Weapon_PPC_PPCER_0-STOCK",ComponentDefType.Weapon)},
            { "Snub-Nose PPC", ("Weapon_PPC_PPCSnub_0-STOCK",ComponentDefType.Weapon)},
            //TAG
            { "TAG", ("Weapon_TAG_Standard_0-STOCK",ComponentDefType.Weapon)},
            //Weapons Ballistic
            { "AC/2", ("Weapon_Autocannon_AC2_0-STOCK",ComponentDefType.Weapon)},
            { "AC/5", ("Weapon_Autocannon_AC5_0-STOCK",ComponentDefType.Weapon)},
            { "AC/10", ("Weapon_Autocannon_AC10_0-STOCK",ComponentDefType.Weapon)},
            { "AC/20", ("Weapon_Autocannon_AC20_0-STOCK",ComponentDefType.Weapon)},
            { "Ultra AC/2", ("Weapon_Autocannon_UAC2_0-STOCK",ComponentDefType.Weapon)},
            { "Ultra AC/5", ("Weapon_Autocannon_UAC5_0-STOCK",ComponentDefType.Weapon)},
            { "Ultra AC/10", ("Weapon_Autocannon_UAC10_0-STOCK",ComponentDefType.Weapon)},
            { "Ultra AC/20", ("Weapon_Autocannon_UAC20_0-STOCK",ComponentDefType.Weapon)},
            { "Gauss Rifle", ("Weapon_Gauss_Gauss_0-STOCK",ComponentDefType.Weapon)},
            { "LB 2-X AC", ("Weapon_Autocannon_LB2X_0-STOCK",ComponentDefType.Weapon)},
            { "LB 5-X AC", ("Weapon_Autocannon_LB5X_0-STOCK",ComponentDefType.Weapon)},
            { "LB 10-X AC", ("Weapon_Autocannon_LB10X_0-STOCK",ComponentDefType.Weapon)},
            { "LB 20-X AC", ("Weapon_Autocannon_LB20X_0-STOCK",ComponentDefType.Weapon)},
            //Flamers
            { "Flamer", ("Weapon_Flamer_Flamer_0-STOCK",ComponentDefType.Weapon)},
            //Missiles
            { "LRM 5", ("Weapon_LRM_LRM5_0-STOCK",ComponentDefType.Weapon)},
            { "LRM 10", ("Weapon_LRM_LRM10_0-STOCK",ComponentDefType.Weapon)},
            { "LRM 15", ("Weapon_LRM_LRM15_0-STOCK",ComponentDefType.Weapon)},
            { "LRM 20", ("Weapon_LRM_LRM20_0-STOCK",ComponentDefType.Weapon)},
            { "SRM 2", ("Weapon_SRM_SRM2_0-STOCK",ComponentDefType.Weapon)},
            { "SRM 4", ("Weapon_SRM_SRM4_0-STOCK",ComponentDefType.Weapon)},
            { "SRM 6", ("Weapon_SRM_SRM6_0-STOCK",ComponentDefType.Weapon)},
            { "Narc Missile Beacon", ("Weapon_Narc_Standard_0-STOCK",ComponentDefType.Weapon)},
            //Ammo
            { "Ammo AC/2", ("Ammo_AmmunitionBox_Generic_AC2",ComponentDefType.AmmunitionBox)},
            { "Ammo AC/5", ("Ammo_AmmunitionBox_Generic_AC5",ComponentDefType.AmmunitionBox)},
            { "Ammo AC/10", ("Ammo_AmmunitionBox_Generic_AC10",ComponentDefType.AmmunitionBox)},
            { "Ammo AC/20", ("Ammo_AmmunitionBox_Generic_AC20",ComponentDefType.AmmunitionBox)},
            { "Ammo Ultra AC/2", ("Ammo_AmmunitionBox_Generic_AC2",ComponentDefType.AmmunitionBox)},
            { "Ammo Ultra AC/5", ("Ammo_AmmunitionBox_Generic_AC5",ComponentDefType.AmmunitionBox)},
            { "Ammo Ultra AC/10", ("Ammo_AmmunitionBox_Generic_AC10",ComponentDefType.AmmunitionBox)},
            { "Ammo Ultra AC/20", ("Ammo_AmmunitionBox_Generic_AC20",ComponentDefType.AmmunitionBox)},
            { "Ammo LB 2-X AC", ("Ammo_AmmunitionBox_Generic_LB2X",ComponentDefType.AmmunitionBox)},
            { "Ammo LB 5-X AC", ("Ammo_AmmunitionBox_Generic_LB5X",ComponentDefType.AmmunitionBox)},
            { "Ammo LB 10-X AC", ("Ammo_AmmunitionBox_Generic_LB10X",ComponentDefType.AmmunitionBox)},
            { "Ammo LB 20-X AC", ("Ammo_AmmunitionBox_Generic_LB20X",ComponentDefType.AmmunitionBox)},
            { "Ammo Gauss Rifle", ("Ammo_AmmunitionBox_Generic_GAUSS",ComponentDefType.AmmunitionBox)},
            { "Machine Gun", ("Ammo_AmmunitionBox_Generic_MG",ComponentDefType.AmmunitionBox)},
            { "Ammo Flamer", ("Ammo_AmmunitionBox_Generic_Flamer",ComponentDefType.AmmunitionBox)},
            { "Ammo LRM 5", ("Ammo_AmmunitionBox_Generic_LRM",ComponentDefType.AmmunitionBox)},
            { "Ammo LRM 10", ("Ammo_AmmunitionBox_Generic_LRM",ComponentDefType.AmmunitionBox)},
            { "Ammo LRM 15", ("Ammo_AmmunitionBox_Generic_LRM",ComponentDefType.AmmunitionBox)},
            { "Ammo LRM 20", ("Ammo_AmmunitionBox_Generic_LRM",ComponentDefType.AmmunitionBox)},
            { "Ammo SRM 2", ("Ammo_AmmunitionBox_Generic_SRM",ComponentDefType.AmmunitionBox)},
            { "Ammo SRM 4", ("Ammo_AmmunitionBox_Generic_SRM",ComponentDefType.AmmunitionBox)},
            { "Ammo SRM 6", ("Ammo_AmmunitionBox_Generic_SRM",ComponentDefType.AmmunitionBox)},
            { "Ammo SRM 8", ("Ammo_AmmunitionBox_Generic_SRM",ComponentDefType.AmmunitionBox)},
            { "Ammo Narc Missile Beacon", ("Ammo_AmmunitionBox_Generic_Narc",ComponentDefType.AmmunitionBox)},

            //Actuators
            { "Shoulder", ("emod_arm_part_shoulder",ComponentDefType.Upgrade)},
            { "Upper Arm Actuator", ("emod_arm_part_upper",ComponentDefType.Upgrade)},
            { "Lower Arm Actuator", ("emod_arm_part_lower",ComponentDefType.Upgrade)},
            { "Hand Actuator", ("emod_arm_part_hand",ComponentDefType.Upgrade)},
            { "Hip", ("emod_leg_hip",ComponentDefType.Upgrade)},
            { "Upper Leg Actuator", ("emod_leg_upper",ComponentDefType.Upgrade)},
            { "Lower Leg Actuator", ("emod_leg_lower",ComponentDefType.Upgrade)},
            { "Foot Actuator", ("emod_leg_foot",ComponentDefType.Upgrade)},
            //HeatSinks
            { "Heat Sink", ("Gear_HeatSink_Generic_Standard",ComponentDefType.HeatSink)},
            { "Double Heat Sink", ("Gear_HeatSink_Generic_Double",ComponentDefType.HeatSink)},
            //Gear
            { "CASE", ("emod_case",ComponentDefType.Upgrade)},
            { "CASE II", ("emod_case2",ComponentDefType.Upgrade)}*/
            //};
            var componentDefDictionary = new Dictionary<string, ComponentDefType>();
            foreach (string files in jsonFiles)
            {
                string[] filelines = System.IO.File.ReadAllLines(files);
                string filename = Path.GetFileName(files);
                string jsonString = File.ReadAllText(files);
                int indexcompontentType = Array.FindIndex(filelines, x => x.Contains("\"ComponentType\""));
                //int indexType = Array.IndexOf(filelines, "\"ComponentType\"");
                //Console.WriteLine(files);
                if (indexcompontentType > -1)
                {
                    //var componentDefType = new ComponentDefType();
                    string cdt = filelines[indexcompontentType];
                    cdt = cdt.RemoveSpecialCharacters();
                    cdt = cdt.Trim();
                    cdt = cdt.Replace("  ", " ");
                    string[] split = cdt.Split(' ');
                    split[1] = split[1].Replace(",", "");
                    //var componentKeyAmmo = new AmmunitionBox();
                    //var ammoDescrption = new Description();
                    try
                    {
                        switch (split[1])
                        {
                            case "AmmunitionBox":
                                {
                                    var componentKeyAmmo = AmmunitionBox.FromJson(jsonString);
                                    //ammoDescrption = componentKeyAmmo.Description;

                                    if (componentKeyAmmo.Description == null) break;
                                    if (componentKeyAmmo.Description.Model == null) break;
                                    if (componentKeyAmmo.Description.UiName.Contains("__")) break;
                                    if (!componentDefDictionary.ContainsKey(componentKeyAmmo.Description.Id))
                                        componentDefDictionary.Add(componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox);
                                    string[] splitAmmo = componentKeyAmmo.Description.UiName.Split(' ');

                                    if (splitAmmo.Length == 2)
                                    {
                                        if (splitAmmo[0].Contains("MG"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + "Machine Gun", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("Gauss"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " Rifle", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("SRM"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 2", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 4", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 6", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("LRM"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 5", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 10", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 15", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0] + " 20", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[1].Contains("Ammo"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + splitAmmo[0], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("Ammo"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[0] + " " + splitAmmo[1], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        componentDefDictionaryTuple.Add("Ammo" + " " + splitAmmo[0], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                        break;
                                    }
                                    if (splitAmmo.Length == 3)
                                    {
                                        if (splitAmmo[0].Contains("LB"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[2] + " " + splitAmmo[0] + " " + splitAmmo[1] + " AC", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("MG"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + "Machine Gun: " + splitAmmo[2], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("Half"))
                                        {
                                            componentDefDictionaryTuple.Add(splitAmmo[1] + " " + "Machine Gun: " + splitAmmo[2], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case "HeatSink":
                                {
                                    var componentKeyHeatSink = HeatSink.FromJson(jsonString);
                                    if (componentKeyHeatSink.Description == null) break;
                                    if (componentKeyHeatSink.Description.UiName == null) break;
                                    if (componentKeyHeatSink.Description.UiName.Contains("__")) break;
                                    if (!componentDefDictionary.ContainsKey(componentKeyHeatSink.Description.Id))
                                        componentDefDictionary.Add(componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink);
                                    string[] splitHeatSink = componentKeyHeatSink.Description.UiName.Split(' ');
                                    if (splitHeatSink.Length == 2)
                                    {
                                        componentDefDictionaryTuple.Add(splitHeatSink[0] + " " + splitHeatSink[1], (componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink));
                                        break;
                                    }
                                    if (splitHeatSink.Length == 3)
                                    {
                                        if (splitHeatSink[2].Contains("CD"))
                                        {
                                            componentDefDictionaryTuple.Add("Clan Double Heat Sink", (componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink));
                                            break;
                                        }
                                        if (splitHeatSink[2].Contains("D"))
                                        {
                                            componentDefDictionaryTuple.Add("Double Heat Sink", (componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink));
                                            break;
                                        }
                                        break;
                                    }
                                    break;
                                }
                            case "JumpJet":
                                {
                                    var componentKeyJumpjet = Jumpjet.FromJson(jsonString);
                                    if (componentKeyJumpjet.Description == null) break;
                                    if (componentKeyJumpjet.Description.UiName == null) break;
                                    if (componentKeyJumpjet.Description.UiName.Contains("__")) break;
                                    if (!componentDefDictionary.ContainsKey(componentKeyJumpjet.Description.Id))
                                        componentDefDictionary.Add(componentKeyJumpjet.Description.Id, ComponentDefType.JumpJet);
                                    break;
                                }
                            case "Upgrade":
                                {
                                    var componentKeyUpgrade = Upgrade.FromJson(jsonString);
                                    if (componentKeyUpgrade.Description == null) break;
                                    if (componentKeyUpgrade.Description.UiName == null) break;
                                    if (componentKeyUpgrade.Description.UiName.Contains("__")) break;
                                    if (!componentDefDictionary.ContainsKey(componentKeyUpgrade.Description.Id))
                                    {
                                        componentDefDictionary.Add(componentKeyUpgrade.Description.Id, ComponentDefType.Upgrade);
                                        componentDefDictionaryTuple.Add(componentKeyUpgrade.Description.UiName, (componentKeyUpgrade.Description.Id, ComponentDefType.Upgrade));
                                        break;
                                    }
                                    break;
                                }
                            case "Weapon":
                                {
                                    var componentKeyWeapon = Weapon.FromJson(jsonString);
                                    if (componentKeyWeapon.Description == null) break;
                                    if (componentKeyWeapon.Description.Model == null) break;
                                    if (componentKeyWeapon.Description.UiName.Contains("__")) break;
                                    if (!componentDefDictionary.ContainsKey(componentKeyWeapon.Description.Id))
                                    {
                                        componentDefDictionary.Add(componentKeyWeapon.Description.Id, ComponentDefType.Weapon);
                                        if (componentKeyWeapon.Description.UiName.Contains("__")) break;
                                        if (componentKeyWeapon.Description.UiName.Contains("UAC"))
                                        {
                                            componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace("UAC", "Ultra AC");
                                        }
                                        if (componentKeyWeapon.Description.UiName.Contains("LB"))
                                        {
                                            componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName + " AC";
                                        }
                                        if (componentKeyWeapon.Description.UiName.Contains("LRM"))
                                        {
                                            string LRM = componentKeyWeapon.Description.UiName.Replace("LRM", "LRM ");
                                            componentDefDictionaryTuple.Add(LRM, (componentKeyWeapon.Description.Id, ComponentDefType.Weapon));
                                            break;
                                        }
                                        if (componentKeyWeapon.Description.UiName.Contains("SRM"))
                                        {
                                            string LRM = componentKeyWeapon.Description.UiName.Replace("SRM", "SRM ");
                                            componentDefDictionaryTuple.Add(LRM, (componentKeyWeapon.Description.Id, ComponentDefType.Weapon));
                                            break;
                                        }
                                        if (componentKeyWeapon.Description.UiName.Contains("Laser") || (componentKeyWeapon.Description.UiName.Contains("Pulse")))
                                        {
                                            string[] splitWeapon = componentKeyWeapon.Description.UiName.Split(' ');
                                            if (splitWeapon[1] == "L")
                                            {
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace(" L ", " Large ");
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace(" M ", " Medium ");
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace(" S ", " Small ");
                                            }
                                            else
                                            {
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace("L ", "Large ");
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace("M ", "Medium ");
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace("S ", "Small ");
                                            }
                                            if (componentKeyWeapon.Description.UiName.Contains("Pulse"))
                                            {
                                                componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace("Pulse", "Pulse Laser");
                                            }
                                        }
                                        if (componentKeyWeapon.Description.UiName.Contains("Snub"))
                                        {
                                            componentKeyWeapon.Description.UiName = componentKeyWeapon.Description.UiName.Replace("Snub", "Snub-Nose");
                                        }
                                        componentDefDictionaryTuple.Add(componentKeyWeapon.Description.UiName, (componentKeyWeapon.Description.Id, ComponentDefType.Weapon));
                                        break;
                                    }
                                    break;
                                }
                        }
                    }
                    catch
                    {

                    }

                }
            }
            String csv = String.Join(Environment.NewLine, componentDefDictionary.Select(d => $"{d.Key},{d.Value}"));
            String csv2 = String.Join(Environment.NewLine, componentDefDictionaryTuple.Select(d => $"{d.Key},{d.Value}"));
            System.IO.File.WriteAllText(Path.Combine(outputDir, "componentDef.csv"), csv);
            System.IO.File.WriteAllText(Path.Combine(outputDir, "componentDefTuple.csv"), csv2);

            //Parallel.ForEach(bedfiles, (currentFile) =>
            foreach (string currentFile in bedfiles)
            {
                string filename = Path.GetFileName(currentFile);
                string[] filelines = System.IO.File.ReadAllLines(currentFile);
                string[] criticalLines = filelines.SkipWhile(x => !x.Contains("Crits"))
                   .Skip(1)
                   .ToArray();
                string[] lines = filelines;
                string[] critLines = criticalLines;
                //Declares
                int[,] cbtISarry = new int[17, 5] { {20,6,5,3,4},{25,8,6,4,6},{30,10,7,5,7},{35,11,8,6,8},
                {40,12,10,6,10},{45,14,11,7,11},{50,16,12,8,12},{55,18,13,9,13},{60,20,14,10,14},
                {65,21,15,10,15},{70,22,15,11,15},{75,23,16,12,16},{80,25,17,13,17},{85,27,18,14,18},
                {90,29,19,15,19},{95,30,20,16,20},{100,31,21,17,21} };
                int[,] cbt2hbsISarry = new int[1, 5] { { 1, 5, 5, 5, 5 } };
                int[,] hbsfromTTISarry = new int[17, 5];
                for (int i = 0; i < hbsfromTTISarry.GetLength(0); i++)
                {
                    for (int a = 0; a < cbt2hbsISarry.Length; a++)
                    {
                        hbsfromTTISarry[i, a] = cbtISarry[i, a] * cbt2hbsISarry[0, a];
                    }
                }
                int totalHeatSinks = 0;
                int installedHeatSinks = 0;
                int tonnageIndex = 0;
                //int chassisDefTonnage = 0;
                var chassisDef = new ChassisDef();
                var mechDef = new MechDef();
                mechDef = new MechDef
                {
                    HeraldryId = null,
                    Description = new DefDescription
                    {
                        Cost = 0,
                        Rarity = 0,
                        Purchasable = true,
                        Manufacturer = null,
                        Model = null,
                        Details = "DETAILS UPDATE",
                    },
                    SimGameMechPartCost = 0,
                    Version = 1,
                    Locations = new List<MechDefLocation>(),
                    Inventory = new List<Inventory>(),
                    MechTags = new Tags
                    {
                        Items = new List<string>
                    {
                        "unit_indirectFire",
                        "unit_mech",
                        "unit_assault",
                        "unit_release",
                        "unit_ready",
                        "unit_lance_tank",
                        "unit_role_brawler",
                        "unit_range_short",
                        "unit_indirectFire",
                        "unit_speed_low",
                        "unit_armor_high",
                        "Davion",
                        "Kurita",
                        "Liao",
                        "Marik",
                        "Steiner"
                    }
                    }
                };
                chassisDef = new ChassisDef
                {
                    FixedEquipment = new List<FixedEquipment>
                    {

                    },
                    Description = new DefDescription
                    {
                        //Needs to be calcuated first
                        Cost = 0,
                        //How do we want to do Rarity?
                        Rarity = 0,
                        Purchasable = true,
                        //Lore Never used
                        Manufacturer = "",
                        //Lore Never Used
                        Model = "",
                        //Display Name in Combat?
                        //UiName = chassisDefDescriptionName,
                        //File name must match saved file name
                        //Id = "chassisdef_" + chassisDefDescriptionName.Replace(" ", "_").ToLower() +
                        //chassisDefVariantName.Replace(" ", "_").ToLower(),
                        //Name = chassisDefDescriptionName,
                        //Details Can be pulled from File Later
                        Details = "DETAILS_UPDATE",
                        //Icon must match SVG,DDS filename
                        //Icon = "uixTxrIcon_" + chassisDefDescriptionName.Replace(" ", "_").ToLower(),
                    },
                    //InitialTonnage = chassisDefTonnage / 10,
                    //Stability = chassisDefTonnage * 2,
                    //MeleeDamage = chassisDefTonnage,
                    //MeleeInstability = chassisDefTonnage,
                    MeleeToHitModifier = 0,
                    //DfaDamage = chassisDefTonnage,
                    DfaToHitModifier = 0,
                    //DfaSelfDamage = chassisDefTonnage,
                    //DfaInstability = chassisDefTonnage,
                    BattleValue = 0,
                    Heatsinks = 0,
                    StabilityDefenses = new List<int>
                {
                    0,0,0,0,0,0
                },
                    SpotterDistanceMultiplier = 1,
                    VisibilityMultiplier = 1,
                    SensorRangeMultiplier = 1,
                    Signature = 0,
                    Radius = 8,
                    PunchesWithLeftArm = false,
                    ChassisTags = new Tags
                    {
                        Items = new List<string>
                        {

                        },
                        TagSetSourceFile = "",
                    },
                    StockRole = "SRUPDATE",
                    YangsThoughts = "YTUpdate",

                    LosSourcePositions = new List<LosPosition>
                {
                    new LosPosition
                    {
                        X = 0.0,
                        Y = 19.0,
                        Z = 0.0
                    },
                    new LosPosition
                    {
                        X = 4.5,
                        Y = 16.0,
                        Z = -0.5
                    },
                    new LosPosition
                    {
                        X = -4.5,
                        Y = 16.0,
                        Z = -0.5
                    }
                },
                    LosTargetPositions = new List<LosPosition>
                {
                    new LosPosition
                    {
                        X = 0.0,
                        Y = 19.0,
                        Z = 0.0
                    },
                    new LosPosition
                    {
                        X = 4.5,
                        Y = 16.0,
                        Z = -0.5
                    },
                    new LosPosition
                    {
                        X = 4.5,
                        Y = 16.0,
                        Z = -0.5
                    },
                    new LosPosition
                    {
                        X = 4.5,
                        Y = 16.0,
                        Z = -0.5
                    },
                    new LosPosition
                    {
                        X = -3.5,
                        Y= 5.5,
                        Z= 1
                    }
                },
                    Locations = new List<ChassisDefLocation>(),
                };
                string shortName = "Nothing";
                if (lines[3].Contains("Biped"))
                {
                    string[] cabCheck = lines[0].Split(',');
                    cabCheck = cabCheck[1].Split(' ');
                    cabCheck[0] = RemoveSpecialCharacters(cabCheck[0]);
                    cabCheck[1] = RemoveSpecialCharacters(cabCheck[1]);
                    if (lines[0].Contains("Adder"))
                    {
                        string something = "break";
                    }
                    if ((preFabDictionary.ContainsKey(cabCheck[1].ToLower())) || (preFabDictionary.ContainsKey(cabCheck[0].ToLower())))
                    {
                        int indexChassisDef = -1;
                        foreach (string l in lines)
                        {
                            string newl = RemoveSpecialCharacters(l);
                            string[] words = newl.Split(',');
                            if (words[0] == "Name")
                            {
                                words = newl.Split(',');
                                string[] split = words[1].Split(' ');

                                switch (split.Length)
                                {
                                    case 5:
                                        if (split[3] == "II")
                                        {
                                            chassisDef.Description.Name = split[0] + " " + split[1] + " " + split[2] + " " + split[3];
                                            chassisDef.VariantName = split[4];
                                            shortName = split[0];
                                            break;
                                        }
                                        chassisDef.Description.Name = split[0] + " " + split[1];
                                        chassisDef.VariantName = split[4];
                                        shortName = split[0];
                                        break;
                                    case 4:
                                        if (split[3] == "Hunter")
                                        {
                                            chassisDef.Description.Name = split[0] + " " + split[1];
                                            chassisDef.VariantName = split[2] + " " + split[3];
                                            shortName = split[0];
                                            break;
                                        }
                                        chassisDef.Description.Name = split[0] + " " + split[1] + " " + split[2];
                                        chassisDef.VariantName = split[3];
                                        shortName = split[0];
                                        break;
                                    case 3:
                                        {
                                            if (split[1] == "Huntsman")
                                            {
                                                chassisDef.Description.Name = split[1] + " " + split[0];
                                                chassisDef.VariantName = split[2];
                                                shortName = split[1];
                                                break;
                                            }
                                            if (split[2] == "Hunter")
                                            {
                                                chassisDef.Description.Name = split[0];
                                                chassisDef.VariantName = split[1] + " " + split[2];
                                                shortName = split[0];
                                                break;
                                            }
                                            if (split[0] == "Puma")
                                            {
                                                chassisDef.Description.Name = split[1];
                                                chassisDef.VariantName = split[2];
                                                shortName = split[1];
                                                break;
                                            }
                                            if (split[1] == "Viper")
                                            {
                                                chassisDef.Description.Name = split[0] + " " + split[1];
                                                chassisDef.VariantName = split[2];
                                                shortName = split[1];
                                                break;
                                            }
                                            if (split[1] == "Executioner")
                                            {
                                                chassisDef.Description.Name = split[1];
                                                chassisDef.VariantName = split[2];
                                                shortName = split[1];
                                                break;
                                            }
                                            if ((split[2] == "Standard") || (split[2].Length == 1))
                                            {
                                                chassisDef.Description.Name = split[0] + " " + split[1];
                                                chassisDef.VariantName = split[2];
                                                shortName = split[0];
                                                break;
                                            }
                                            //Default 3s
                                            chassisDef.Description.Name = split[1] + " " + split[2];
                                            chassisDef.VariantName = split[0];
                                            shortName = split[1];
                                            break;
                                        }
                                    default: //Default 2s
                                        if (split[1] == "Standard")
                                        {
                                            chassisDef.Description.Name = split[0] + " " + split[1];
                                            chassisDef.VariantName = split[1];
                                            shortName = split[0];
                                            break;
                                        }
                                        if (split[1].Length == 1)
                                        {
                                            chassisDef.Description.Name = split[0];
                                            chassisDef.VariantName = split[1];
                                            shortName = split[0];
                                            break;
                                        }
                                        chassisDef.Description.Name = split[1];
                                        chassisDef.VariantName = split[0];
                                        shortName = split[1];
                                        break;
                                }

                                string preloadName = chassisDef.Description.Name;
                                string preloadVariantName = chassisDef.VariantName;
                                chassisDef.Description.Id = "chassisdef_" + chassisDef.Description.Name.Replace(" ", "_").ToLower() +
                                    "_" + chassisDef.VariantName.Replace(" ", "_");
                                mechDef.Description.UiName = chassisDef.Description.UiName + " " + chassisDef.VariantName;
                                mechDef.Description.Id = chassisDef.Description.Id.Replace("chassisdef", "mechdef");
                                mechDef.Description.Name = chassisDef.Description.Name;
                                mechDef.Description.Icon = chassisDef.Description.Icon;
                                //Perfect Match
                                indexChassisDef = Array.FindIndex(chassisDefFiles, x => x.Contains(chassisDef.Description.Id));
                                //Variant Match (Used for Hero mechs)
                                if (indexChassisDef == -1)
                                {
                                    if (chassisDef.VariantName.Length > 2) indexChassisDef = Array.FindIndex(chassisDefFiles, x => x.Contains(chassisDef.VariantName));
                                }
                                //Fallback
                                if ((generateFromChassis == true) && (indexChassisDef == -1))
                                {
                                    string fallback = chassisDef.VariantName;
                                    while ((indexChassisDef == -1) && (fallback.Length > 1))
                                    {
                                        if (fallback.Length == 2)
                                        {
                                            fallback = chassisDef.Description.Id.Remove(chassisDef.Description.Id.Length - 1, 1);
                                        }
                                        else
                                        {
                                            fallback = fallback.Remove(fallback.Length - 1, 1);
                                        }
                                        if (indexChassisDef == -1) indexChassisDef = Array.FindIndex(chassisDefFiles, x => x.Contains(fallback));
                                    }
                                    if (indexChassisDef == -1) indexChassisDef = Array.FindIndex(chassisDefFiles, x => x.Contains(shortName.ToLower()));
                                }
                                if ((generateFromChassis == false) || (indexChassisDef == -1))
                                {
                                    chassisDef.Description.Icon = "uixTxrIcon_" + shortName.ToLower();
                                    chassisDef.HardpointDataDefId = "hardpointdatadef_" + shortName.ToLower();
                                    chassisDef.PrefabIdentifier = "chrPrfMech_" + shortName.ToLower() + "Base-001";
                                    chassisDef.PrefabBase = shortName.ToLower();
                                }
                                if ((generateFromChassis == true) && (indexChassisDef > -1))
                                {
                                    string jsonString = File.ReadAllText(Path.Combine(settings.ChassisDefpath, chassisDefFiles[indexChassisDef]));
                                    chassisDef = ChassisDef.FromJson(jsonString);
                                    mechDef.Description.Details = chassisDef.Description.Details;
                                    mechDef.Description.Cost = chassisDef.Description.Cost;
                                    mechDef.Description.Rarity = chassisDef.Description.Rarity;
                                    chassisDef.Description.Name = preloadName;
                                    chassisDef.Description.Id = "chassisdef_" + chassisDef.Description.Name.Replace(" ", "_").ToLower() +
                                        "_" + chassisDef.VariantName.Replace(" ", "_");
                                }
                                mechDef.ChassisId = chassisDef.Description.Id;
                            }
                            for (int i = 0; i < hbsfromTTISarry.GetLength(0); i++)
                            {
                                if (hbsfromTTISarry[i, 0] == chassisDef.Tonnage)
                                {
                                    tonnageIndex = i;
                                    //Console.WriteLine(hbsfromTTISarry[i, 0]);
                                    chassisDef.MovementCapDefId = "movedef_assaultmech";
                                    chassisDef.PathingCapDefId = "pathingdef_assault";
                                    chassisDef.WeightClass = "ASSAULT";
                                    if (chassisDef.Tonnage < 80)
                                    {
                                        chassisDef.MovementCapDefId = "movedef_heavymech";
                                        chassisDef.PathingCapDefId = "pathingdef_heavy";
                                        chassisDef.WeightClass = "HEAVY";
                                    }
                                    if (chassisDef.Tonnage < 60)
                                    {
                                        chassisDef.MovementCapDefId = "movedef_mediummech";
                                        chassisDef.PathingCapDefId = "pathingdef_medium";
                                        chassisDef.WeightClass = "MEDIUM";
                                    }
                                    if (chassisDef.Tonnage < 40)
                                    {
                                        chassisDef.MovementCapDefId = "movedef_lightmech";
                                        chassisDef.PathingCapDefId = "pathingdef_light";
                                        chassisDef.WeightClass = "LIGHT";
                                    }
                                    break;
                                }
                            }
                            if (words[0].ToLower().Contains("tons"))
                            {
                                if ((generateFromChassis == false) || (indexChassisDef == -1))
                                {
                                    chassisDef.Tonnage = Convert.ToInt32(words[1]);
                                    chassisDef.InitialTonnage = chassisDef.Tonnage / 10;
                                    chassisDef.Stability = chassisDef.Tonnage * 2;
                                    chassisDef.MeleeDamage = chassisDef.Tonnage;
                                    chassisDef.MeleeInstability = chassisDef.Tonnage;
                                    chassisDef.MeleeToHitModifier = 0;
                                    chassisDef.DfaDamage = chassisDef.Tonnage;
                                    chassisDef.DfaToHitModifier = 0;
                                    chassisDef.DfaSelfDamage = chassisDef.Tonnage;
                                    chassisDef.DfaToHitModifier = 0;
                                    chassisDef.DfaInstability = chassisDef.Tonnage;
                                }
                            }
                            if (words[0].ToLower().Contains("armorvals"))
                            {
                                if ((generateFromChassis == false) || (indexChassisDef == -1))
                                {
                                    //string[] split = words[1].Split(',');
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.Head,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 4,
                                        MaxArmor = 45,
                                        MaxRearArmor = -1,
                                        InternalStructure = 16,
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.LeftArm,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 12,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 3] * 2,
                                        MaxRearArmor = -1,
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 3],
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.RightArm,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 12,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 3] * 2,
                                        MaxRearArmor = -1,
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 3],
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.LeftTorso,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 12,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 2] * 2,
                                        MaxRearArmor = hbsfromTTISarry[tonnageIndex, 2],
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.RightTorso,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 12,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 2] * 2,
                                        MaxRearArmor = hbsfromTTISarry[tonnageIndex, 2],
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.CenterTorso,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 16,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 1] * 2,
                                        MaxRearArmor = hbsfromTTISarry[tonnageIndex, 1],
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 1],
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.LeftLeg,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 6,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 4] * 2,
                                        MaxRearArmor = -1,
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                    });
                                    chassisDef.Locations.Add(new ChassisDefLocation
                                    {
                                        Location = Location.RightLeg,
                                        Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        }
                                    },
                                        Tonnage = 0.0,
                                        InventorySlots = 6,
                                        MaxArmor = hbsfromTTISarry[tonnageIndex, 4] * 2,
                                        MaxRearArmor = -1,
                                        InternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                    });
                                }
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.Head,
                                    CurrentArmor = 45,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = 16,
                                    AssignedArmor = 45,
                                    AssignedRearArmor = -1,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.LeftArm,
                                    CurrentArmor = Convert.ToInt32(words[1]) * 5,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 3],
                                    AssignedArmor = Convert.ToInt32(words[1]) * 5,
                                    AssignedRearArmor = -1,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.RightArm,
                                    CurrentArmor = Convert.ToInt32(words[1]) * 5,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 3],
                                    AssignedArmor = Convert.ToInt32(words[1]) * 5,
                                    AssignedRearArmor = -1,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.LeftTorso,
                                    CurrentArmor = Convert.ToInt32(words[3]) * 5,
                                    CurrentRearArmor = Convert.ToInt32(words[6]) * 5,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                    AssignedArmor = Convert.ToInt32(words[3]) * 5,
                                    AssignedRearArmor = Convert.ToInt32(words[6]) * 5,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.RightTorso,
                                    CurrentArmor = Convert.ToInt32(words[3]) * 5,
                                    CurrentRearArmor = Convert.ToInt32(words[6]) * 5,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                    AssignedArmor = Convert.ToInt32(words[3]) * 5,
                                    AssignedRearArmor = Convert.ToInt32(words[6]) * 5,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.CenterTorso,
                                    CurrentArmor = Convert.ToInt32(words[5]) * 5,
                                    CurrentRearArmor = Convert.ToInt32(words[8]) * 5,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 1],
                                    AssignedArmor = Convert.ToInt32(words[5]) * 5,
                                    AssignedRearArmor = Convert.ToInt32(words[8]) * 5,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.LeftLeg,
                                    CurrentArmor = Convert.ToInt32(words[10]) * 5,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                    AssignedArmor = Convert.ToInt32(words[10]) * 5,
                                    AssignedRearArmor = -1,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.RightLeg,
                                    CurrentArmor = Convert.ToInt32(words[10]) * 5,
                                    CurrentRearArmor = -1,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                    AssignedArmor = Convert.ToInt32(words[10]) * 5,
                                    AssignedRearArmor = -1,
                                });
                            }
                            if (words[0] == "Engine")
                            {
                                string[] split = words[3].Split('/');
                                if ((generateFromChassis == false) || (indexChassisDef == -1))
                                {


                                    if (split[1].Contains("("))
                                    {
                                        string[] split2 = split[1].Split('(');
                                        chassisDef.TopSpeed = Convert.ToInt32(split2[0]) * 30;
                                        chassisDef.TurnRadius = 90;
                                        chassisDef.MaxJumpjets = Convert.ToInt32(split[2]);
                                    }
                                    else
                                    {
                                        chassisDef.TopSpeed = Convert.ToInt32(split[1]) * 30;
                                        chassisDef.TurnRadius = 90;
                                        chassisDef.MaxJumpjets = Convert.ToInt32(split[2]);
                                    }
                                }
                                string engineType = "emod_engineslots_std_center";
                                if (words[1] == "XL")
                                {
                                    engineType = "emod_engineslots_xl_center";
                                    mechDef.Inventory.Add(new Inventory
                                    {
                                        MountedLocation = Location.CenterTorso,
                                        ComponentDefId = engineType,
                                        ComponentDefType = ComponentDefType.HeatSink,
                                        HardpointSlot = -1,
                                        DamageLevel = "Functional",
                                        PrefabName = null,
                                        HasPrefabName = false,
                                        SimGameUid = "",
                                        Guid = null
                                    });
                                    mechDef.Inventory.Add(new Inventory
                                    {
                                        MountedLocation = Location.LeftTorso,
                                        ComponentDefId = "emod_engineslots_size3",
                                        ComponentDefType = ComponentDefType.HeatSink,
                                        HardpointSlot = -1,
                                        DamageLevel = "Functional",
                                        PrefabName = null,
                                        HasPrefabName = false,
                                        SimGameUid = "",
                                        Guid = null
                                    });
                                    mechDef.Inventory.Add(new Inventory
                                    {
                                        MountedLocation = Location.RightTorso,
                                        ComponentDefId = "emod_engineslots_size3",
                                        ComponentDefType = ComponentDefType.HeatSink,
                                        HardpointSlot = -1,
                                        DamageLevel = "Functional",
                                        PrefabName = null,
                                        HasPrefabName = false,
                                        SimGameUid = "",
                                        Guid = null
                                    });
                                }
                                int engineRating = Convert.ToInt32(split[0]);
                                mechDef.Inventory.Add(new Inventory
                                {
                                    MountedLocation = Location.CenterTorso,
                                    ComponentDefId = "emod_engine_" + engineRating * chassisDef.Tonnage,
                                    ComponentDefType = ComponentDefType.HeatSink,
                                    HardpointSlot = -1,
                                    DamageLevel = "Functional",
                                    PrefabName = null,
                                    HasPrefabName = false,
                                    SimGameUid = "",
                                    Guid = null
                                });
                            }
                            if (words[0] == "Sinks")
                            {
                                string hsType = "emod_kit_shs";
                                if (words[1] == "Double")
                                {
                                    hsType = "emod_kit_dhs";
                                }
                                mechDef.Inventory.Add(new Inventory
                                {
                                    MountedLocation = Location.CenterTorso,
                                    ComponentDefId = hsType,
                                    ComponentDefType = ComponentDefType.HeatSink,
                                    HardpointSlot = -1,
                                    DamageLevel = "Functional",
                                    PrefabName = null,
                                    HasPrefabName = false,
                                    SimGameUid = "",
                                    Guid = null
                                });
                                totalHeatSinks = Convert.ToInt32(words[2]) - 10;
                            }
                            if (words[0] == "Gyro")
                            {
                                string gyroType = null;
                                if (words[1] == "XL")
                                {
                                    gyroType = "Gear_Gyro_XL";
                                }
                                if (words[1] == "Standard")
                                {
                                    gyroType = "Gear_Gyro_Generic_Standard";
                                }
                                if (gyroType != null)
                                {
                                    mechDef.Inventory.Add(new Inventory
                                    {
                                        MountedLocation = Location.CenterTorso,
                                        ComponentDefId = gyroType,
                                        ComponentDefType = ComponentDefType.Upgrade,
                                        HardpointSlot = -1,
                                        DamageLevel = "Functional",
                                        PrefabName = null,
                                        HasPrefabName = false,
                                        SimGameUid = "",
                                        Guid = null
                                    });
                                }
                            }
                            if (words[0] == "Internal")
                            {
                                string structureType = "emod_structureslots_standard";
                                if (words[1] == "Endo-Steel")
                                {
                                    structureType = "emod_structureslots_endosteel";
                                }
                                mechDef.Inventory.Add(new Inventory
                                {
                                    MountedLocation = Location.CenterTorso,
                                    ComponentDefId = structureType,
                                    ComponentDefType = ComponentDefType.Upgrade,
                                    HardpointSlot = -1,
                                    DamageLevel = "Functional",
                                    PrefabName = null,
                                    HasPrefabName = false,
                                    SimGameUid = "",
                                    Guid = null
                                });
                            }
                            if (words[0].Contains("Crits"))
                            {
                                //critProcessing = true;
                                for (int i = 0; i < critLines.Length; i++)
                                {

                                    var mountLocationVar = Location.LeftArm;
                                    if (i <= 11) mountLocationVar = Location.LeftArm;
                                    if ((i <= 23) && (i > 11)) mountLocationVar = Location.LeftTorso;
                                    if ((i <= 35) && (i > 23)) mountLocationVar = Location.RightTorso;
                                    if ((i <= 47) && (i > 35)) mountLocationVar = Location.RightArm;
                                    if ((i <= 59) && (i > 49)) mountLocationVar = Location.CenterTorso;
                                    if ((i <= 65) && (i > 59)) mountLocationVar = Location.Head;
                                    if ((i <= 71) && (i > 65)) mountLocationVar = Location.LeftLeg;
                                    if ((i <= 77) && (i > 71)) mountLocationVar = Location.RightLeg;
                                    critLines[i] = critLines[i].Replace("\"", "");
                                    string[] split = critLines[i].Split(',');
                                    if (componentDefDictionaryTuple.ContainsKey(split[0]))
                                    {
                                        if (!(split[0].Contains("Endo") || split[0].Contains("Ferro") || split[0].Contains("Gyro") || split[0].Contains("Sensors")))
                                        {
                                            mechDef.Inventory.Add(new Inventory
                                            {
                                                MountedLocation = mountLocationVar,
                                                ComponentDefId = componentDefDictionaryTuple[split[0]].Item1,
                                                ComponentDefType = componentDefDictionaryTuple[split[0]].Item2,
                                                HardpointSlot = -1,
                                                DamageLevel = "Functional",
                                                PrefabName = null,
                                                HasPrefabName = false,
                                                SimGameUid = "",
                                                Guid = null
                                            });
                                        }
                                    }
                                    if (split[0].Contains("Heat Sink")) installedHeatSinks++;
                                }
                                if (installedHeatSinks < totalHeatSinks)
                                {
                                    int engineHS = totalHeatSinks - installedHeatSinks;
                                    mechDef.Inventory.Add(new Inventory
                                    {
                                        MountedLocation = Location.CenterTorso,
                                        ComponentDefId = "emod_engine_cooling_" + engineHS,
                                        ComponentDefType = ComponentDefType.HeatSink,
                                        HardpointSlot = -1,
                                        DamageLevel = "Functional",
                                        PrefabName = null,
                                        HasPrefabName = false,
                                        SimGameUid = "",
                                        Guid = null
                                    });
                                }
                            }
                        }
                        string outputmechDef = Newtonsoft.Json.JsonConvert.SerializeObject(mechDef, Newtonsoft.Json.Formatting.Indented, BattleEngineJsonCreation.Converter.Settings);
                        string outputchassisDef = Newtonsoft.Json.JsonConvert.SerializeObject(chassisDef, Newtonsoft.Json.Formatting.Indented, BattleEngineJsonCreation.Converter.Settings);
                        File.WriteAllText(Path.Combine(outputDir, chassisDef.Description.Id + ".json"), outputchassisDef);
                        File.WriteAllText(Path.Combine(outputDir, mechDef.Description.Id + ".json"), outputmechDef);
                        indexChassisDef = -1;
                    }
                }
            }
        }
        private static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ',' || c == '/' || c == ' ' || c == '-')
                {
                    sb.Append(c);
                }
            }
            //Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
        private static void EndProgram(string reason)
        {
            Console.WriteLine(reason);
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
