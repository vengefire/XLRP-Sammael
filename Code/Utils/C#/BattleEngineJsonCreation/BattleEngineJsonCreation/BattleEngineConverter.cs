using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace BattleEngineJsonCreation
{
    class BattleEngineConverter
    {
        static void Main(string[] args)
        {
            var filepath = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(filepath, "*.bed", SearchOption.AllDirectories);
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
            int tonnageIndex=0;
            //try
            //{
                Parallel.ForEach(files,(currentFile) =>
                //foreach (var currentFile in files)
                {
                    string filename = Path.GetFileName(currentFile);
                    string[] lines = System.IO.File.ReadAllLines(currentFile);
                    if (lines[3].Contains("Biped"))
                    {
                        Console.WriteLine($"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}");
                        dynamic chassisDef = new ChassisDef
                        {
                            Locations = new List<ChassisDefLocation>(),
                            Description = new DefDescription()
                        };
                        //chassisDefDescription = new DefDescription();
                        //mechDefDescription = new DefDescription();
                        dynamic mechDef = new MechDef
                        {
                            Locations = new List<MechDefLocation>(),
                            Description = new DefDescription()
                        };
                        chassisDef.FixedEquipment = new List<FixedEquipment> { };
                        var chassisDefTags = new Tags { };
                        chassisDef.Description.Purchasable = true;
                        chassisDef.Description.Manufacturer = "";
                        chassisDef.Description.Model = "";
                        chassisDef.SpotterDistanceMultiplier = 1;
                        chassisDef.VisibilityMultiplier = 1;
                        chassisDef.Signature = 0;
                        chassisDef.Radius = 8;
                        chassisDef.PunchesWithLeftArm = false;
                        chassisDefTags.TagSetSourceFile = "";
                        chassisDef.Heatsinks = 0;
                        chassisDef.StockRole = "SRUPDATE";
                        chassisDef.YangsThoughts = "YTUPDATE";

                        foreach (string l in lines)
                        {
                            string newl = l.Replace("\"", "");
                            string[] words = newl.Split(',');
                            if (words[0].ToLower().Contains("name"))
                            {
                                string[] split = words[1].Split(' ');
                                if (split.Length > 2)
                                {
                                    chassisDef.VariantName = split[0];
                                    chassisDef.Description.Name = split[1] + " " + split[2];
                                    mechDef.Description.Name = split[1] + " " + split[2];
                                }
                                if (split.Length > 3)
                                {
                                    chassisDef.VariantName = split[0];
                                    chassisDef.Description.Name = split[3] + " " + split[1] + " " + split[2];
                                    mechDef.Description.Name = split[3] + " " + split[1] + " " + split[2];
                                }
                                if (split.Length == 2)
                                {
                                    chassisDef.VariantName = split[0];
                                    chassisDef.Description.Name = split[1];
                                    mechDef.Description.Name = split[1];
                                }
                                chassisDef.Description.Id = "chassisdef_" + chassisDef.Description.Name
                                + "_" + chassisDef.VariantName;
                                chassisDef.Description.Icon = "uixTxrIcon_" + chassisDef.Description.Name.ToLower();
                                chassisDef.HardpointDataDefId = "hardpointdatadef_" + chassisDef.Description.Name.ToLower();
                                chassisDef.PrefabIdentifier = "chrPrfMech_" + chassisDef.Description.Name.ToLower() + "Base-001";
                                chassisDef.PrefabBase = chassisDef.Description.Name.ToLower();
                            }
                            if (words[0].ToLower().Contains("tons"))
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
                            }

                            if (words[0].ToLower().Contains("armorvals"))
                            {
                                //string[] split = words[1].Split(',');
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.Head,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 4,
                                    MaxArmor = 45,
                                    MaxRearArmor = -1,
                                    InternalStructure = 16,
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.LeftArm,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 12,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 4] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.RightArm,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 12,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 4] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.LeftTorso,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 12,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 2] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.RightTorso,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 12,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 2] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.CenterTorso,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 16,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 1] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 1],
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.LeftLeg,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 16,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 4] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                });
                                chassisDef.Locations.Add(new ChassisDefLocation
                                {
                                    Location = Location.RightLeg,
                                    //Hardpoints = 
                                    Tonnage = 0.0,
                                    InventorySlots = 16,
                                    MaxArmor = hbsfromTTISarry[tonnageIndex, 4] * 2,
                                    MaxRearArmor = -1,
                                    InternalStructure = hbsfromTTISarry[tonnageIndex, 4],
                                });
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
                                    AssignedArmor = Convert.ToInt32(words[6]) * 5,
                                    AssignedRearArmor = -1,
                                });
                                mechDef.Locations.Add(new MechDefLocation
                                {
                                    DamageLevel = DamageLevel.Functional,
                                    Location = Location.RightTorso,
                                    CurrentArmor = Convert.ToInt32(words[3]) * 5,
                                    CurrentRearArmor = Convert.ToInt32(words[6]) * 5,
                                    CurrentInternalStructure = hbsfromTTISarry[tonnageIndex, 2],
                                    AssignedArmor = Convert.ToInt32(words[6]) * 5,
                                    AssignedRearArmor = -1,
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
                        }
                        string outputmechDef = Newtonsoft.Json.JsonConvert.SerializeObject(mechDef, Newtonsoft.Json.Formatting.Indented);
                        string outputchassisDef = Newtonsoft.Json.JsonConvert.SerializeObject(chassisDef, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText("mechdef_" + filename + ".json", outputmechDef);
                        File.WriteAllText("chassisdef_" + filename + ".json", outputchassisDef);
                    }
                    else
                    {
                        //Console.WriteLine($"Skiped {filename} on thread {Thread.CurrentThread.ManagedThreadId}");
                        //Console.ReadKey();
                    }


                    });
                //}
            //}
            /*catch (System.Exception excpt)
            {
                    Console.WriteLine(excpt.Message);
                    Console.ReadKey();
            } */       
        }
        public void ParseEngine(string[] lines)
        {

        }
    }
}
