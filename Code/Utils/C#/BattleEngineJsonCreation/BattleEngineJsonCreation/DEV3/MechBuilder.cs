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
        public static ChassisDef ChassisDefs(string[] chassisNames, string[] files)
        {
            var chassisDef = new ChassisDef();
            string toSearch = chassisNames[0].ToLower() + "_" + chassisNames[1].ToUpper();
            int index = -1;
            string filename;
            foreach (string file in files)
            {
                filename = Path.GetFileName(file);
                if (file.Contains(chassisNames[1]))
                {
                    string jsonString = File.ReadAllText(file);
                    chassisDef = ChassisDef.FromJson(jsonString);
                }
            }
            return chassisDef;
        }
        public static MechDef MechDefs(ChassisDef chassisDef)
        {
            
            var mechdef = new MechDef
            {
                ChassisId = chassisDef.Description.Id,
                HeraldryId = null,
                Description = new DefDescription
                {
                    Cost = chassisDef.Description.Cost,
                    Rarity = chassisDef.Description.Rarity,
                    UiName = chassisDef.Description.UiName + " " + chassisDef.VariantName,
                    Id = chassisDef.Description.Id.Replace("chassisdef", "mechdef"),
                    Name = chassisDef.Description.Name + "_" + chassisDef.VariantName,
                    Icon = chassisDef.Description.Icon
                },
                Locations = new List<MechDefLocation>(),
                Inventory = new List<Inventory>(),
            };
            foreach (Location location in Enum.GetValues(typeof(Location)))
            {
                mechdef.Locations.Add(new MechDefLocation
                {
                    DamageLevel = DamageLevel.Functional,
                    Location = location,
                    CurrentArmor = MechISArmor(chassisDef.Tonnage, location, true),
                    CurrentRearArmor = MechISArmor(chassisDef.Tonnage, location, true),
                    AssignedArmor = MechISArmor(chassisDef.Tonnage, location, false),
                    AssignedRearArmor = MechISArmor(chassisDef.Tonnage, location, true)
                });
            }
            return mechdef;
        }
        public static MechDef MechLocations(Dictionary<string, (string, ComponentDefType)> componentDefDictionaryTuple, MechDef mechdef, string file)
        {
            string[] filelines = System.IO.File.ReadAllLines(file);
            string[] critLines = filelines.SkipWhile(x => !x.Contains("Crits"))
            .Skip(1)
            .ToArray();
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
                        mechdef.Inventory.Add(new Inventory
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
            }
            return mechdef;
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
        public static double MechISArmor(double tons, Location location, bool rear)
        {
            int index = (int)location + 2;
            double vaule = 0;
            int[,] cbtISarray = new int[17, 5] { {20,6,5,3,4},{25,8,6,4,6},{30,10,7,5,7},{35,11,8,6,8},
                {40,12,10,6,10},{45,14,11,7,11},{50,16,12,8,12},{55,18,13,9,13},{60,20,14,10,14},
                {65,21,15,10,15},{70,22,15,11,15},{75,23,16,12,16},{80,25,17,13,17},{85,27,18,14,18},
                {90,29,19,15,19},{95,30,20,16,20},{100,31,21,17,21} };
            int[,] cbt2hbsISarray = new int[1, 5] { { 1, 5, 5, 5, 5 } };
            int[,] hbsfromTTISarray = new int[17, 5];
            for (int i = 0; i < hbsfromTTISarray.GetLength(0); i++)
            {
                for (int a = 0; a < cbt2hbsISarray.Length; a++)
                {
                    //hbsfromTTISarray[i, a] = cbt2hbsISarray[i, a] * cbt2hbsISarray[0, a];
                    if ((i == tons) && (a == (int)location))
                    {
                        if (rear == false) vaule = cbtISarray[Array.IndexOf(cbtISarray, tons), index];
                    }
                }
            }
            return vaule;
        }

    }
}
