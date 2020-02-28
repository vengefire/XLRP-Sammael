using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    public static class MechBuilder
    {
        public static bool CabCheck(Dictionary<string, string> preFabDictionary, string file)
        {
            bool result = false;
            string[] filelines = System.IO.File.ReadAllLines(file);
            if (filelines[3].Contains("Biped"))
            {
                var cabCheck = new List<string>(Reuse.CabCheck(filelines[0]));
                for (int i = 0; i < cabCheck.Count; i++)
                {
                    if (preFabDictionary.ContainsKey(cabCheck[i]))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public static string[] ChassisName(string filename)
        {
            string chassisName;
            string chassisVariantName;
            string chassisShortName;
            string[] filelines = System.IO.File.ReadAllLines(filename);
            string[] words = Reuse.RemoveSpecialCharacters(filelines[0]).Split(',');
            string[] split = words[1].Split(' ');
            switch (split.Length)
            {
                case 5:
                    if (split[3] == "II")
                    {
                        chassisName = split[0] + " " + split[1] + " " + split[2] + " " + split[3];
                        chassisVariantName = split[4];
                        chassisShortName = split[0];

                        break;
                    }
                    chassisName = split[0] + " " + split[1];
                    chassisVariantName = split[4];
                    chassisShortName = split[0];
                    break;
                case 4:
                    if (split[3] == "Hunter")
                    {
                        chassisName = split[0] + " " + split[1];
                        chassisVariantName = split[2] + " " + split[3];
                        chassisShortName = split[0];
                        break;
                    }
                    chassisName = split[0] + " " + split[1] + " " + split[2];
                    chassisVariantName = split[3];
                    chassisShortName = split[0];
                    break;
                case 3:
                    {
                        if (split[1] == "Huntsman")
                        {
                            chassisName = split[1] + " " + split[0];
                            chassisVariantName = split[2];
                            chassisShortName = split[1];
                            break;
                        }
                        if (split[2] == "Hunter")
                        {
                            chassisName = split[0];
                            chassisVariantName = split[1] + " " + split[2];
                            chassisShortName = split[0];
                            break;
                        }
                        if (split[0] == "Puma")
                        {
                            chassisName = split[1];
                            chassisVariantName = split[2];
                            chassisShortName = split[1];
                            break;
                        }
                        if (split[1] == "Viper")
                        {
                            chassisName = split[0] + " " + split[1];
                            chassisVariantName = split[2];
                            chassisShortName = split[1];
                            break;
                        }
                        if (split[1] == "Executioner")
                        {
                            chassisName = split[1];
                            chassisVariantName = split[2];
                            chassisShortName = split[1];
                            break;
                        }
                        if ((split[2] == "Standard") || (split[2].Length == 1))
                        {
                            chassisName = split[0] + " " + split[1];
                            chassisVariantName = split[2];
                            chassisShortName = split[0];
                            break;
                        }
                        //Default 3s
                        chassisName = split[1] + " " + split[2];
                        chassisVariantName = split[0];
                        chassisShortName = split[1];
                        break;
                    }
                default: //Default 2s
                    if (split[1] == "Standard")
                    {
                        chassisName = split[0] + " " + split[1];
                        chassisVariantName = split[1];
                        chassisShortName = split[0];
                        break;
                    }
                    if (split[1].Length == 1)
                    {
                        chassisName = split[0];
                        chassisVariantName = split[1];
                        chassisShortName = split[0];
                        break;
                    }
                    chassisName = split[1];
                    chassisVariantName = split[0];
                    chassisShortName = split[1];
                    break;
            }
            return new string[] { chassisName, chassisVariantName, chassisShortName };
        }
        public static ChassisDef ChassisDefs(string[] chassisNames, string[] files, bool rebuild)
        {
            var chassisDef = new ChassisDef();
            string toSearch = chassisNames[0].ToLower() + "_" + chassisNames[1].ToUpper();
            //int index = -1;
            string filename;
            foreach (string file in files)
            {
                filename = Path.GetFileName(file);
                string[] split = filename.Split('_');
                foreach (string s in split)
                {
                    string news = s.Replace(".json", "");
                    if ((news == (chassisNames[1])) && (rebuild == false))
                    {
                        string jsonString = File.ReadAllText(file);
                        chassisDef = ChassisDef.FromJson(jsonString);
                        if (chassisDef.Description.Name.Contains("__"))
                        {
                            LocalizationParse(chassisDef, file);
                        }
                    }
                    //Rebuild is an attempt to rebuild ChassisDefs if they are missing for variants BETA Use Wisely. 
                    if ((file.Contains(chassisNames[0])) && (rebuild == true))
                    {
                        string jsonString = File.ReadAllText(file);
                        chassisDef = ChassisDef.FromJson(jsonString);
                        break;
                    }
                }
            }
            return chassisDef;
        }
        private static ChassisDef LocalizationParse(ChassisDef chassisDef, string file)
        {
            string jsonString = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Localization.json"));
            var localization = Localization.FromJson(jsonString);
            foreach (var obj in localization)
            {
                chassisDef.Description.UiName = chassisDef.Description.UiName.Replace("__/", "");
                chassisDef.Description.UiName = chassisDef.Description.UiName.Replace("/__", "");
                chassisDef.Description.Name = chassisDef.Description.Name.Replace("__/", "");
                chassisDef.Description.Name = chassisDef.Description.Name.Replace("/__", "");
                chassisDef.Description.Details = chassisDef.Description.Details.Replace("__/", "");
                chassisDef.Description.Details = chassisDef.Description.Details.Replace("/__", "");
                if (chassisDef.Description.UiName == obj.Name)
                {
                    chassisDef.Description.UiName = obj.Original;
                }
                if (chassisDef.Description.Name == obj.Name)
                {
                    chassisDef.Description.Name = obj.Original;
                }
                if (chassisDef.Description.Details == obj.Name)
                {
                    chassisDef.Description.Details = obj.Original;
                }
            }
            string outputchassisDef = Newtonsoft.Json.JsonConvert.SerializeObject(chassisDef, Newtonsoft.Json.Formatting.Indented, Converter.Settings);
            File.WriteAllText(file, outputchassisDef);

            return chassisDef;
        }
        public static MechDef MechDefs(ChassisDef chassisDef, string bedfile)
        {

            var mechdef = new MechDef
            {
                ChassisId = chassisDef.Description.Id,
                HeraldryId = null,
                Description = new DefDescription
                {
                    Cost = chassisDef.Description.Cost,
                    Rarity = chassisDef.Description.Rarity,
                    UiName = chassisDef.Description.UiName,
                    Id = chassisDef.Description.Id.Replace("chassisdef", "mechdef"),
                    Name = chassisDef.Description.Name,
                    Details = chassisDef.Description.Details,
                    Icon = chassisDef.Description.Icon
                },
                Locations = new List<MechDefLocation>(),
                Inventory = new List<Inventory>(),
            };
            string[] filelines = File.ReadAllLines(bedfile);
            string armorline = "";
            int internalStructre = -1;
            foreach (string lines in filelines)
            {
                string newl = Reuse.RemoveSpecialCharacters(lines);
                newl = newl.Replace("\"", "");
                if (newl.Contains("ArmorVals"))
                {
                    armorline = newl;
                    break;
                }
            }
            if (!armorline.Contains("ArmorVals")) Reuse.EndProgram("FATAL ERROR: Unable to parse ArmorVals from Bedfile");
            string[] armorvaulewords = armorline.Split(',');
            foreach (Location location in Enum.GetValues(typeof(Location)))
            {
                //ArmorVaule to Location  If Array
                int frontarmorvaule = -1;
                int reararmorvaule = -1;
                if ((int)location == 0)
                {
                    frontarmorvaule = 45;
                }
                else
                {
                    frontarmorvaule = Convert.ToInt32(armorvaulewords[(int)location]) * 5;
                }
                if (((int)location == 3) || ((int)location == 4))
                    reararmorvaule = Convert.ToInt32(armorvaulewords[6]) * 5;
                if ((int)location == 5) reararmorvaule = Convert.ToInt32(armorvaulewords[8]) * 5;
                if (((int)location == 6) || ((int)location == 7)) frontarmorvaule = Convert.ToInt32(armorvaulewords[10]) * 5;
                foreach (Location chassislocation in Enum.GetValues(typeof(Location)))
                {
                    if (chassisDef.Locations[(int)chassislocation].Location == location)
                    {
                        internalStructre = chassisDef.Locations[(int)chassislocation].InternalStructure;
                    }
                }
                mechdef.Locations.Add(new MechDefLocation
                {
                    DamageLevel = DamageLevel.Functional,
                    Location = location,
                    CurrentArmor = frontarmorvaule,
                    CurrentRearArmor = reararmorvaule,
                    CurrentInternalStructure = internalStructre,
                    AssignedArmor = frontarmorvaule,
                    AssignedRearArmor = reararmorvaule,
                });
            }
            return mechdef;
        }
        public static MechDef MechLocations(Dictionary<string, (string, ComponentDefType)> componentDefDictionaryTuple, MechDef mechdef, string file, ChassisDef chassisDef)
        {
            string[] filelines = System.IO.File.ReadAllLines(file);
            string[] critLines = filelines.SkipWhile(x => !x.Contains("Crits"))
            .Skip(1)
            .ToArray();
            bool addedDHS = false;
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
                if ((componentDefDictionaryTuple.ContainsKey(split[0])) || (split[0].Contains("Jump Jet")))
                {
                    if (!(split[0].Contains("Endo") || split[0].Contains("XL") || split[0].Contains("Ferro") || split[0].Contains("Gyro") || split[0].Contains("Sensors") || split[0].Contains("Life")))
                    {
                        string compDefId = "";
                        var componentDefType = new ComponentDefType();
                        if (split[0].Contains("Jump Jet"))
                        {
                            componentDefType = ComponentDefType.JumpJet;
                            if (chassisDef.Tonnage <= 90) compDefId = "Gear_JumpJet_Generic_Assault";
                            if (chassisDef.Tonnage <= 60) compDefId = "Gear_JumpJet_Generic_Heavy";
                            if (chassisDef.Tonnage <= 35) compDefId = "Gear_JumpJet_Generic_Standard";
                        }
                        else
                        {
                            compDefId = componentDefDictionaryTuple[split[0]].Item1;
                            componentDefType = componentDefDictionaryTuple[split[0]].Item2;
                        }
                        if (chassisDef.Description.UiName.Contains("Wolf"))
                        {
                            string something = "broke";
                        }
                        if ((split[0].Contains("Double Heat Sink")) && (addedDHS == false))
                        {
                            mechdef.Inventory.Add(new Inventory
                            {
                                MountedLocation = Location.CenterTorso,
                                ComponentDefId = "emod_kit_dhs",
                                ComponentDefType = ComponentDefType.HeatSink,
                                HardpointSlot = -1,
                                DamageLevel = "Functional",
                                PrefabName = null,
                                HasPrefabName = false,
                                SimGameUid = "",
                                Guid = null
                            });
                            addedDHS = true;
                        }
                        mechdef.Inventory.Add(new Inventory
                        {
                            MountedLocation = mountLocationVar,
                            ComponentDefId = compDefId,
                            ComponentDefType = componentDefType,
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
            return mechdef;
        }
        public static MechDef Engines(ChassisDef chassisDef, MechDef mechdef, string bedfile)
        {
            string[] filelines = File.ReadAllLines(bedfile);
            int enginerating = 1;
            string engineString = "emod_engine_";
            foreach (string lines in filelines)
            {
                string newl = lines.Replace("\"", "");
                if (newl.Contains("Engine"))
                {
                    string[] split = newl.Split(',');
                    split = split[3].Split('/');
                    enginerating = Convert.ToInt32(split[0]) * Convert.ToInt32(chassisDef.Tonnage);
                    if (enginerating == 0)
                    {
                        string something = "broken";
                    }
                    if (enginerating < 100) engineString = "emod_engine_0";
                    mechdef.Inventory.Add(new Inventory
                    {
                        MountedLocation = Location.CenterTorso,
                        ComponentDefId = engineString + enginerating,
                        ComponentDefType = ComponentDefType.HeatSink,
                        HardpointSlot = -1,
                        DamageLevel = "Functional",
                        PrefabName = null,
                        HasPrefabName = false,
                        SimGameUid = "",
                        Guid = null
                    });
                    break;
                }
            }
            return mechdef;
        }
        public static MechDef Tags(MechDef mechdef)
        {
            mechdef.MechTags = new Tags
            {
                Items = new List<string>
                {
                    "unit_release"
                }
            };
            return mechdef;
        }

        public static double MechISArmor(double tons, Location location, bool rear)
        {
            int index = (int)location + 2;
            double vaule = 0;
            //Tons=0, CT=1, ST=2, LEGS=3, ARMS=4
            int[,] cbtISarray = new int[17, 5] { {20,6,5,3,4},{25,8,6,4,6},{30,10,7,5,7},{35,11,8,6,8},
                {40,12,10,6,10},{45,14,11,7,11},{50,16,12,8,12},{55,18,13,9,13},{60,20,14,10,14},
                {65,21,15,10,15},{70,22,15,11,15},{75,23,16,12,16},{80,25,17,13,17},{85,27,18,14,18},
                {90,29,19,15,19},{95,30,20,16,20},{100,31,21,17,21} };
            //int[,] cbt2hbsISarray = new int[1, 5] { { 1, 5, 5, 5, 5 } };
            //int[,] hbsfromTTISarray = new int[17, 5];
            //Loop to match chassisTonnage as tons int i
            for (int i = 0; i < cbtISarray.GetLength(0); i++)
            {
                if ((cbtISarray[i, 0] == tons))
                {
                    //Once tonnage is foud loop to find index position matching location index
                    //Head is 0, Vaules based on order of Enum from ChassisDef Class should match array.
                    for (int a = 0; a < 5; a++)
                    {
                        //hbsfromTTISarray[i, a] = cbt2hbsISarray[i, a] * cbt2hbsISarray[0, a];
                        if ((int)location == 0)
                        {
                            vaule = 45;
                            break;
                        }
                        if (a == (int)location)
                        {
                            if (rear == false) vaule = cbtISarray[i, a] * 5;
                            if (rear == true) vaule = cbtISarray[i, a] * 5;
                        }

                    }
                }



            }
            return vaule;
        }

    }
}
