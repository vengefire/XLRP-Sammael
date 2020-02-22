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
            int tonnageIndex = 0;

            //BED Name, (HBS weaponID "File name without.json", ComponentDefType { AmmunitionBox, HeatSink, JumpJet, Upgrade, Weapon }; 
            var componentDefDictionary = new Dictionary<string, (string, ComponentDefType)>() {
                                    //Weapons Laser
                                    { "Large Laser", ("Weapon_Laser_LargeLaser_0-STOCK",ComponentDefType.Weapon)},
                                    { "Medium Laser", ("Weapon_Laser_MediumLaser_0-STOCK",ComponentDefType.Weapon)},
                                    { "Small Laser", ("Weapon_Laser_SmallLaser_0-STOCK",ComponentDefType.Weapon)},
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
                                    //Weapons Ballistic
                                    { "AC/2", ("Weapon_Autocannon_AC2_0-STOCK",ComponentDefType.Weapon)},
                                    { "AC/5", ("Weapon_Autocannon_AC5_0-STOCK",ComponentDefType.Weapon)},
                                    { "AC/10", ("Weapon_Autocannon_AC10_0-STOCK",ComponentDefType.Weapon)},
                                    { "AC/20", ("Weapon_Autocannon_AC20_0-STOCK",ComponentDefType.Weapon)},
                                    { "Gauss Rifle", ("Weapon_Gauss_Gauss_0-STOCK",ComponentDefType.Weapon)},
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
                                    //Ammo
                                    { "Ammo AC/2", ("Ammo_AmmunitionBox_Generic_AC2",ComponentDefType.AmmunitionBox)},
                                    { "Ammo AC/5", ("Ammo_AmmunitionBox_Generic_AC5",ComponentDefType.AmmunitionBox)},
                                    { "Ammo AC/10", ("Ammo_AmmunitionBox_Generic_AC10",ComponentDefType.AmmunitionBox)},
                                    { "Ammo AC/20", ("Ammo_AmmunitionBox_Generic_AC20",ComponentDefType.AmmunitionBox)},
                                    { "Ammmo Gauss Rifle", ("Ammo_AmmunitionBox_Generic_GAUSS",ComponentDefType.AmmunitionBox)},
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
                                    };

            Parallel.ForEach(files, (currentFile) =>
            //foreach (var currentFile in files)
            {
                string filename = Path.GetFileName(currentFile);
                string[] lines = System.IO.File.ReadAllLines(currentFile);
                string[] critLines = lines.SkipWhile(x => !x.Contains("Crits"))
                   .Skip(1)
                   .ToArray();
                dynamic chassisDef = new ChassisDef
                {
                    Locations = new List<ChassisDefLocation>(),
                    Description = new DefDescription(),
                    StabilityDefenses = new List<long>(),
                    LosTargetPositions = new List<LosPosition>(),
                    LosSourcePositions = new List<LosPosition>(),
                };
                //chassisDefDescription = new DefDescription();
                //mechDefDescription = new DefDescription();
                dynamic mechDef = new MechDef
                {
                    Locations = new List<MechDefLocation>(),
                    Description = new DefDescription(),
                    Inventory = new List<FixedEquipment>(),
                };
                int totalHeatSinks = 0;
                int installedHeatSinks = 0;
                if (lines[3].Contains("Biped"))
                //&&(lines[0].Contains("ARC-2K")))
                {
                    //Console.WriteLine($"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}");

                    chassisDef.FixedEquipment = new List<FixedEquipment> { };
                    var chassisDefTags = new Tags { };
                    chassisDef.Description.Cost = 666666;
                    chassisDef.Description.Rarity = 5;
                    chassisDef.Description.Purchasable = true;
                    chassisDef.Description.Manufacturer = "";
                    chassisDef.Description.Model = "";
                    chassisDef.SpotterDistanceMultiplier = 1;
                    chassisDef.VisibilityMultiplier = 1;
                    chassisDef.SensorRangeMultiplier = 1;
                    chassisDef.Signature = 0;
                    chassisDef.Radius = 8;
                    chassisDef.PunchesWithLeftArm = false;
                    chassisDefTags.TagSetSourceFile = "";
                    chassisDef.Heatsinks = 0;
                    chassisDef.StockRole = "SRUPDATE";
                    chassisDef.YangsThoughts = "YTUPDATE";
                    for (int i = 0; i < 6; i++)
                    {
                        chassisDef.StabilityDefenses.Add(0);
                    }
                    chassisDef.LosSourcePositions.Add(new LosPosition
                    {
                        X = 0.0,
                        Y = 19.0,
                        Z = 0.0
                    });
                    chassisDef.LosSourcePositions.Add(new LosPosition
                    {
                        X = 4.5,
                        Y = 16.0,
                        Z = -0.5
                    });
                    chassisDef.LosSourcePositions.Add(new LosPosition
                    {
                        X = -4.5,
                        Y = 16.0,
                        Z = -0.5
                    });
                    chassisDef.LosTargetPositions.Add(new LosPosition
                    {
                        X = 0.0,
                        Y = 19.0,
                        Z = 0.0
                    });
                    chassisDef.LosTargetPositions.Add(new LosPosition
                    {
                        X = 4.5,
                        Y = 16.0,
                        Z = -0.5
                    });
                    chassisDef.LosTargetPositions.Add(new LosPosition
                    {
                        X = -4.5,
                        Y = 16.0,
                        Z = -0.5
                    });
                    chassisDef.LosTargetPositions.Add(new LosPosition
                    {
                        X = -3.0,
                        Y = 5.5,
                        Z = 1.0
                    });
                    //bool critProcessing = false;
                    foreach (string l in lines)
                    {
                        string newl = l.Replace("\"", "");
                        //string remove = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";
                        //newl = Regex.Replace(newl, remove, "");
                        newl = newl.Replace("(", "");
                        newl = newl.Replace(")", "");
                        string[] words = newl.Split(',');
                        if (words[0].ToLower().Contains("name"))
                        {
                            words[1] = words[1].Replace("/", "");
                            string[] split = words[1].Split(' ');
                            if (split.Length > 2)
                            {
                                chassisDef.VariantName = split[0];
                                chassisDef.Description.Name = split[1] + " " + split[2];
                                mechDef.Description.Name = split[1] + " " + split[2];
                            }
                            if (split.Length > 3)
                            {
                                chassisDef.VariantName = split[3];
                                chassisDef.Description.Name = split[0] + " " + split[1] + " " + split[2];
                                mechDef.Description.Name = split[0] + " " + split[1] + " " + split[2];
                            }
                            if (split.Length > 4)
                            {
                                chassisDef.VariantName = split[4];
                                chassisDef.Description.Name = split[0] + " " + split[1] + " " + split[2] + " " + split[3];
                                mechDef.Description.Name = split[0] + " " + split[1] + " " + split[2] + " " + split[3];
                            }
                            if (split.Length == 2)
                            {
                                chassisDef.VariantName = split[0];
                                chassisDef.Description.Name = split[1];
                                mechDef.Description.Name = split[1];
                            }
                            chassisDef.Description.UiName = chassisDef.Description.Name;
                            string chassisDefVariantName = chassisDef.VariantName.Replace(" ", "_").ToLower();
                            string chassisDefDescriptionName = chassisDef.Description.Name.Replace(" ", "_").ToLower();
                            //chassisDef.VariantName = chassisDef.VariantName.Replace(" ", "");
                            //chassisDef.VariantName = chassisDef.VariantName.Replace(" ", "");

                            chassisDef.Description.Id = "chassisdef_" + chassisDefDescriptionName
                           + "_" + chassisDefVariantName;
                            chassisDef.Description.Icon = "uixTxrIcon_" + chassisDefDescriptionName;
                            //chassisDef.HardpointDataDefId = "hardpointdatadef_" + chassisDefDescriptionName;
                            chassisDef.HardpointDataDefId = "hardpointdatadef_atlas";
                            chassisDef.PrefabIdentifier = "chrPrfMech_" + chassisDefDescriptionName + "Base-001";
                            chassisDef.PrefabBase = chassisDefDescriptionName;
                            mechDef.ChassisId = chassisDef.Description.Id;
                            mechDef.HeraldryId = null;
                            mechDef.Description.Cost = 666666666;
                            mechDef.Description.Rarity = 0;
                            mechDef.Description.Purchasable = true;
                            mechDef.Description.Manufacturer = null;
                            mechDef.Description.Model = null;
                            mechDef.Description.UiName = chassisDef.Description.Name + " " + chassisDef.VariantName;
                            mechDef.Description.Id = "mechdef_" + chassisDefDescriptionName + "_" + chassisDefVariantName;
                            mechDef.Description.Name = chassisDef.Description.Name;
                            mechDef.Description.Details = "MECHDEFDETAILSUPDATE";
                            mechDef.Description.Icon = "uixTxrIcon_" + chassisDefDescriptionName;
                            mechDef.SimGameMechPartCost = 666666666;
                            mechDef.Version = 1.0;

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
                                Hardpoints = new List<Hardpoint>
                                    {
                                        new Hardpoint 
                                        {
                                            WeaponMount = WeaponMount.AntiPersonnel,
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
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                    },
                                Tonnage = 0.0,
                                InventorySlots = 16,
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
                                            WeaponMount = WeaponMount.Energy,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Missile,
                                            Omni = true,
                                        },
                                        new Hardpoint
                                        {
                                            WeaponMount = WeaponMount.Ballistic,
                                            Omni = true,
                                        },
                                    },
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
                            string engineType = "emod_engineslots_std_center";
                            if (words[1] == "XL")
                            {
                                engineType = "emod_engineslots_xl_center";
                                mechDef.Inventory.Add(new FixedEquipment
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
                                mechDef.Inventory.Add(new FixedEquipment
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
                                mechDef.Inventory.Add(new FixedEquipment
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
                        }
                        if (words[0] == "Sinks")
                        {
                            string hsType = "emod_kit_shs";
                            if (words[1] == "Double")
                            {
                                hsType = "emod_kit_dhs";
                            }
                            mechDef.Inventory.Add(new FixedEquipment
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
                                mechDef.Inventory.Add(new FixedEquipment
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
                            mechDef.Inventory.Add(new FixedEquipment
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
                                if (componentDefDictionary.ContainsKey(split[0]))
                                {
                                    mechDef.Inventory.Add(new FixedEquipment
                                    {
                                        MountedLocation = mountLocationVar,
                                        ComponentDefId = componentDefDictionary[split[0]].Item1,
                                        ComponentDefType = componentDefDictionary[split[0]].Item2,
                                        HardpointSlot = -1,
                                        DamageLevel = "Functional",
                                        PrefabName = null,
                                        HasPrefabName = false,
                                        SimGameUid = "",
                                        Guid = null
                                    });
                                }
                                if (split[0] == "Heat Sink") installedHeatSinks++;
                            }
                            if (installedHeatSinks != totalHeatSinks)
                            {
                                int engineHS = totalHeatSinks - installedHeatSinks;
                                mechDef.Inventory.Add(new FixedEquipment
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
                    string chassisDefVariantName2 = chassisDef.VariantName.Replace(" ", "_").ToLower();
                    string chassisDefDescriptionName2 = chassisDef.Description.Name.Replace(" ", "_").ToLower();
                    string outputmechDef = Newtonsoft.Json.JsonConvert.SerializeObject(mechDef, Newtonsoft.Json.Formatting.Indented, BattleEngineJsonCreation.Converter.Settings);
                    string outputchassisDef = Newtonsoft.Json.JsonConvert.SerializeObject(chassisDef, Newtonsoft.Json.Formatting.Indented, BattleEngineJsonCreation.Converter.Settings);
                    File.WriteAllText("mechdef_" + chassisDefDescriptionName2 + "_" + chassisDefVariantName2 + ".json", outputmechDef);
                    File.WriteAllText("chassisdef_" + chassisDefDescriptionName2 + "_" + chassisDefVariantName2 + ".json", outputchassisDef);
                }
            });
            //}
        }
        public void ParseEngine(string[] lines)
        {

        }
    }
}
