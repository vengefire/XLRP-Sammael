using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    static class BattleEngineConverter
    {
        static void Main()
        {

            //Gets Application current directory
            string filepath = Directory.GetCurrentDirectory();
            //Gets all files *.bed in all sub directories.
            string[] files = Directory.GetFiles(filepath, "*.bed", SearchOption.AllDirectories);
            Parallel.ForEach(files, (currentFile) =>
            //foreach (string currentFile in files)
            {
                string filename = Path.GetFileName(currentFile);
                string[] filelines = System.IO.File.ReadAllLines(currentFile);
                string[] criticalLines = filelines.SkipWhile(x => !x.Contains("Crits"))
                   .Skip(1)
                   .ToArray();
                FileBEDProcessing(filelines, criticalLines);
                });
            //}
        }
        private static void FileBEDProcessing(this string[] lines, string[] critLines)
        {
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
                                    { "CASE II", ("emod_case2",ComponentDefType.Upgrade)}
                                    };
            //Have CAB for
            var cabDictionary = new Dictionary<string, string>
            {
                //DLCs
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
                //CAB
                {"adder","chrprfmech_adderbase-001"},
                {"arce","chrprfmech_arcebase-001"},
                {"archangel","chrprfmech_archangelbase-001"},
                {"arcticcheetah","chrprfmech_arcticcheetahbase-001"},
                {"arcticwolf","chrprfmech_arcticwolfbase-001"},
                {"avatar","chrprfmech_avatarbase-001"},
                {"blackbeard","chrprfmech_blackbeardbase-001"},
                {"blacklanner","chrprfmech_blacklannerbase-001"},
                {"bloodasp","chrprfmech_bloodaspbase-001"},
                {"bombard","chrprfmech_bombardbase-001"},
                {"bumb","chrprfmech_bumbbase-001"},
                {"bushwacker","chrprfmech_bushwackerbase-001"},
                {"champion","chrprfmech_championbase-001"},
                {"charger","chrprfmech_chargerhotd-001"},
                {"chargerpb","chrprfmech_chargerpbhotd-001"},
                {"clif","chrprfmech_clifbase-001"},
                {"clint","chrprfmech_clintbase-001"},
                {"cougar","chrprfmech_cougarbase-001"},
                {"crusader","chrprfmech_crusaderhotd-001"},
                {"dervish","chrprfmech_dervishbase-001"},
                {"direwolf","chrprfmech_direwolfbase-001"},
                {"ebonjaguar","chrprfmech_ebonjaguarbase-001"},
                {"elemental","chrprfmech_elementalbase-001"},
                {"emperor","chrprfmech_emperorbase-001"},
                {"executioner","chrprfmech_executionerbase-001"},
                {"fafnir","chrprfmech_fafnirbase-001"},
                {"firebee","chrprfmech_firebeebase-001"},
                {"gargoyle","chrprfmech_gargoylebase-001"},
                {"grimlock","chrprfmech_grimlockbase-001"},
                {"groovey","chrprfmech_grooveybase-001"},
                {"hatamotochi","chrprfmech_hatamotochibase-001"},
                {"hauptmann","chrprfmech_hauptmannbase-001"},
                {"helepolis","chrprfmech_helepolisbase-001"},
                {"hellbringer","chrprfmech_hellbringerbase-001"},
                {"hellfire","chrprfmech_hellfirebase-001"},
                {"hellhound","chrprfmech_hellhoundbase-001"},
                {"hellspawn","chrprfmech_hellspawnbase-001"},
                {"highlanderiic","chrprfmech_highlanderiicbase-001"},
                {"hollander","chrprfmech_hollanderbase-001"},
                {"hunchbackiic","chrprfmech_hunchbackiichotd-001"},
                {"huntsman","chrprfmech_huntsmanbase-001"},
                {"huronw","chrprfmech_huronwbase-001"},
                {"icarusii","chrprfmech_icarusiibase-001"},
                {"iceferret","chrprfmech_iceferretbase-001"},
                {"incubus","chrprfmech_incubusbase-001"},
                {"jenneriic","chrprfmech_jenneriicbase-001"},
                {"juggernaut","chrprfmech_juggernautbase-001"},
                {"k2pult","chrprfmech_k2pultbase-001"},
                {"k9","chrprfmech_k9base-001"},
                {"kanazuchi","chrprfmech_kanazuchibase-001"},
                {"kanazuchis","chrprfmech_kanazuchisbase-001"},
                {"kitfox","chrprfmech_kitfoxbase-001"},
                {"kodiak","chrprfmech_kodiakbase-001"},
                {"lament","chrprfmech_lamentbase-001"},
                {"linebacker","chrprfmech_linebackerbase-001"},
                {"longbow","chrprfmech_longbowbase-001"},
                {"lupus","chrprfmech_lupusbase-001"},
                {"mackie","chrprfmech_mackiebase-001"},
                {"madcat","chrprfmech_madcatbase-001"},
                {"madcatmkii","chrprfmech_madcatmkiibase-001"},
                {"maddog","chrprfmech_maddogbase-001"},
                {"marauderii","chrprfmech_marauderiibase-001"},
                {"marauderiic","chrprfmech_marauderiicbase-001"},
                {"mauler","chrprfmech_maulerbase-001"},
                {"mira","chrprfmech_mirabase-001"},
                {"mistlynx","chrprfmech_mistlynxbase-001"},
                {"mongoose","chrprfmech_mongoosebase-001"},
                {"nightgyr","chrprfmech_nightgyrbase-001"},
                {"nightstar","chrprfmech_nightstarbase-001"},
                {"nova","chrprfmech_novabase-001"},
                {"novacat","chrprfmech_novacatbase-001"},
                {"opt","chrprfmech_optbase-001"},
                {"orioniic","chrprfmech_orioniicbase-001"},
                {"osiris","chrprfmech_osirisbase-001"},
                {"ostroc","chrprfmech_ostrochotd-001"},
                {"ostsol","chrprfmech_ostsolhotd-001"},
                {"ostwar","chrprfmech_ostwarhotd-001"},
                {"phawklam","chrprfmech_phawklambase-001"},
                {"piranha","chrprfmech_piranhabase-001"},
                {"preta","chrprfmech_pretabase-001"},
                {"redreaper","chrprfmech_redreaperbase-001"},
                {"riflemaniic","chrprfmech_riflemaniicbase-001"},
                {"roughneck","chrprfmech_roughneckbase-001"},
                {"roughneckcrane-001","chrprfmech_roughneckcrane-001"},
                {"rtcorsair1","chrprfmech_rtcorsair1base-001"},
                {"rtcorsair2","chrprfmech_rtcorsair2base-001"},
                {"rtcorsair3","chrprfmech_rtcorsair3base-001"},
                {"rtcorsair4","chrprfmech_rtcorsair4base-001"},
                {"rtcorsair5","chrprfmech_rtcorsair5base-001"},
                {"shadowcat","chrprfmech_shadowcatbase-001"},
                {"shankey","chrprfmech_shankeybase-001"},
                {"shawklam","chrprfmech_shawklambase-001"},
                {"side","chrprfmech_sidebase-001"},
                {"star","chrprfmech_starbase-001"},
                {"stinger","chrprfmech_stingerhotd-001"},
                {"stingerlam","chrprfmech_stingerlambase-001"},
                {"stonerhino","chrprfmech_stonerhinobase-001"},
                {"stormcrow","chrprfmech_stormcrowbase-001"},
                {"summoner","chrprfmech_summonerbase-001"},
                {"sun","chrprfmech_sunbase-001"},
                {"sunspider","chrprfmech_sunspiderbase-001"},
                {"supernova","chrprfmech_supernovabase-001"},
                {"swav","chrprfmech_swavbase-001"},
                {"talos","chrprfmech_talosbase-001"},
                {"templar","chrprfmech_templarbase-001"},
                {"thanatos","chrprfmech_thanatosbase-001"},
                {"urbanmech50","chrprfmech_urbanmech50base-001"},
                {"urbie","chrprfmech_urbiebase-001"},
                {"uziel","chrprfmech_uzielbase-001"},
                {"valiant","chrprfmech_valiantbase-001"},
                {"valkyrie","chrprfmech_valkyriehotd-001"},
                {"valkyrieii","chrprfmech_valkyrieiibase-001"},
                {"vaporeagle","chrprfmech_vaporeaglebase-001"},
                {"viper","chrprfmech_viperbase-001"},
                {"warhammeriic","chrprfmech_warhammeriicbase-001"},
                {"warhawk","chrprfmech_warhawkbase-001"},
                {"wasp","chrprfmech_wasphotd-001"},
                {"wasplam","chrprfmech_wasplambase-001"},
                {"wolfhound","chrprfmech_wolfhoundbase-001"},
                {"xenorauder","chrprfmech_xenorauderbase-001"},
                //Base
                {"atlas","chrprfmech_atlasbase-001"},
                {"atlas_bha","chrprfmech_atlas_bhabase-001"},
                {"awesome","chrprfmech_awesomebase-001"},
                {"banshee","chrprfmech_bansheebase-001"},
                {"battlemaster","chrprfmech_battlemasterbase-001"},
                {"blackjack","chrprfmech_blackjackbase-001"},
                {"blackknight","chrprfmech_blackknightbase-001"},
                {"cataphract","chrprfmech_cataphractbase-001"},
                {"catapult","chrprfmech_catapultbase-001"},
                {"centurion","chrprfmech_centurionbase-001"},
                {"cicada","chrprfmech_cicadabase-001"},
                {"commando","chrprfmech_commandobase-001"},
                {"dragon","chrprfmech_dragonbase-001"},
                {"enforcer","chrprfmech_enforcerbase-001"},
                {"firestarter","chrprfmech_firestarterbase-001"},
                {"grasshopper","chrprfmech_grasshopperbase-001"},
                {"griffin","chrprfmech_griffinbase-001"},
                {"highlander","chrprfmech_highlanderbase-001"},
                {"highlander_sldf","chrprfmech_highlander_sldfbase-001"},
                {"hunchback","chrprfmech_hunchbackbase-001"},
                {"jagermech","chrprfmech_jagermechbase-001"},
                {"jenner","chrprfmech_jennerbase-001"},
                {"kingcrab","chrprfmech_kingcrabbase-001"},
                {"kintaro","chrprfmech_kintarobase-001"},
                {"locust","chrprfmech_locustbase-001"},
                {"marauder","chrprfmech_marauderbase-001"},
                {"marauder_bha","chrprfmech_marauder_bhabase-001"},
                {"marauder_blackwidow","chrprfmech_marauder_blackwidowbase-001"},
                {"orion","chrprfmech_orionbase-001"},
                {"panther","chrprfmech_pantherbase-001"},
                {"quickdraw","chrprfmech_quickdrawbase-001"},
                {"shadowhawk","chrprfmech_shadowhawkbase-001"},
                {"spider","chrprfmech_spiderbase-001"},
                {"stalker","chrprfmech_stalkerbase-001"},
                {"thunderbolt","chrprfmech_thunderboltbase-001"},
                {"trebuchet","chrprfmech_trebuchetbase-001"},
                {"urbanmech","chrprfmech_urbanmechbase-001"},
                {"victor","chrprfmech_victorbase-001"},
                {"vindicator","chrprfmech_vindicatorbase-001"},
                {"warhammer","chrprfmech_warhammerbase-001"},
                {"warhammer_blackwidow","chrprfmech_warhammer_blackwidowbase-001"},
                {"wolverine","chrprfmech_wolverinebase-001"},
                {"zeus","chrprfmech_zeusbase-001"}
            };
            int totalHeatSinks = 0;
            int installedHeatSinks = 0;
            int tonnageIndex = 0;
            //int chassisDefTonnage = 0;
            var chassisDef = new ChassisDef
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
            var mechDef = new MechDef
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
            if (lines[3].Contains("Biped"))
            {
                string[] cabCheck = lines[0].Split(',');
                cabCheck = cabCheck[1].Split(' ');
                cabCheck[0] = RemoveSpecialCharacters(cabCheck[0]);
                cabCheck[1] = RemoveSpecialCharacters(cabCheck[1]);
                if (lines[0].Contains("Hellhound"))
                {
                    string something = "break";
                }
                if ((cabDictionary.ContainsKey(cabCheck[1].ToLower())) || (cabDictionary.ContainsKey(cabCheck[0].ToLower())))
                {
                    foreach (string l in lines)
                    {
                        string newl = RemoveSpecialCharacters(l);
                        string[] words = newl.Split(',');
                        if (words[0] == "Name")
                        {
                            words = newl.Split(',');
                            string[] split = words[1].Split(' ');
                            string shortName = "Nothing";
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
                                    if ((split[1] == "Standard") || (split[1].Length == 1))
                                    {
                                        chassisDef.Description.Name = split[0] + " " + split[1];
                                        chassisDef.VariantName = split[1];
                                        shortName = split[0];
                                        break;
                                    }
                                    chassisDef.Description.Name = split[1];
                                    chassisDef.VariantName = split[0];
                                    shortName = split[1];
                                    break;
                            }
                            chassisDef.Description.UiName = chassisDef.Description.Name;
                            chassisDef.Description.Id = "chassisdef_" + chassisDef.Description.Name.Replace(" ", "_").ToLower() +
                                "_" + chassisDef.VariantName.Replace(" ", "_");

                            chassisDef.Description.Icon = "uixTxrIcon_" + shortName.ToLower();
                            chassisDef.HardpointDataDefId = "hardpointdatadef_" + shortName.ToLower();
                            chassisDef.PrefabIdentifier = "chrPrfMech_" + shortName.ToLower() + "Base-001";
                            chassisDef.PrefabBase = shortName.ToLower();
                            
                            mechDef.ChassisId = chassisDef.Description.Id;
                            mechDef.Description.UiName = chassisDef.Description.UiName + " " + chassisDef.VariantName;
                            mechDef.Description.Id = "mechdef_" + chassisDef.Description.Name.Replace(" ", "_").ToLower() +
                                "_" + chassisDef.VariantName.Replace(" ", "_");
                            mechDef.Description.Name = chassisDef.Description.Name;
                            mechDef.Description.Icon = "uixTxrIcon_" + shortName.ToLower();

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
                                ComponentDefId = "emod_engine_"+engineRating*chassisDef.Tonnage,
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
                                if (componentDefDictionary.ContainsKey(split[0]))
                                {
                                    mechDef.Inventory.Add(new Inventory
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
                    File.WriteAllText(mechDef.Description.Id + ".json", outputmechDef);
                    File.WriteAllText(chassisDef.Description.Id + ".json", outputchassisDef);
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
    }
}
