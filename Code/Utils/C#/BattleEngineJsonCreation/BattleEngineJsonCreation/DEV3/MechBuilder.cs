using System;
using System.IO;

namespace BattleEngineJsonCreation
{
    public static class MechBuilder
    {
        public static ChassisDef ChassisDefs(string[] chassisNames, string[] files)
        {
            var chassisdef = new ChassisDef();
            int index = Array.IndexOf(files, chassisNames[0] + "_" + chassisNames[1]);
            if (index == -1) index = Array.IndexOf(files, chassisNames[2] + "_" + chassisNames[1]);
            if (index == -1) index = Array.IndexOf(files, chassisNames[2] + "_" + chassisNames[0]);
            if (index > -1)
            {
                string jsonString = File.ReadAllText(files[index]);
                chassisdef = ChassisDef.FromJson(jsonString);
            }
            return chassisdef;
        }
        public static BattleEngineJsonCreation.MechDef MechDefs(ChassisDef chassisDef, string file)
        {
            var mechdef = new MechDef();
            mechdef.ChassisId = chassisDef.Description.Id;
            mechdef.HeraldryId = null;
            mechdef.Description.Cost = chassisDef.Description.Cost;
            mechdef.Description.Rarity = chassisDef.Description.Rarity;
            mechdef.Description = chassisDef.Description;
            mechdef.Description.UiName = chassisDef.Description.UiName + " " + chassisDef.VariantName;
            mechdef.Description.Id = chassisDef.Description.Id.Replace("chassisdef", "mechdef");
            mechdef.Description.Name = chassisDef.Description.Name + "_" + chassisDef.VariantName;
            mechdef.Description.Icon = chassisDef.Description.Icon;
            foreach(Location location in Enum.GetValues(typeof(Location)))
            {
                mechdef.Locations.Add(new MechDefLocation
                {
                    DamageLevel = DamageLevel.Functional,
                    Location = location,
                    CurrentArmor = MechISArmor(chassisDef.Tonnage.HasValue(), location,true),
                    CurrentRearArmor = MechISArmor(chassisDef.Tonnage, location, true),
                    AssignedArmor = MechISArmor(chassisDef.Tonnage, location, false),
                    AssignedRearArmor= MechISArmor(chassisDef.Tonnage, location, true)
                })
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
        public static double MechISArmor(double tons,Location location, bool rear)
        {
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
            return 
        }

    }
}
