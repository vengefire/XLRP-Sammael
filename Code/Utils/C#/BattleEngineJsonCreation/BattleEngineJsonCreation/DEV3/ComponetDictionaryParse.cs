using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    public class ComponetDictionaryParse
    {
        public static Dictionary<string, (string, ComponentDefType)> ComDefDicPop(string[] file)
        {
            var cDDic = new Dictionary<string, ComponentDefType>();
            var CDDicT = new Dictionary<string, (string, ComponentDefType)>();
            foreach (string files in file)
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
                    cdt = Reuse.RemoveSpecialCharacters(cdt);
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
                                    var componentKeyAmmo = BattleEngineJsonCreation.AmmunitionBox.FromJson(jsonString);
                                    //ammoDescrption = componentKeyAmmo.Description;

                                    if (componentKeyAmmo.Description == null) break;
                                    if (componentKeyAmmo.Description.Model == null) break;
                                    if (componentKeyAmmo.Description.UiName.Contains("__")) break;
                                    if (!cDDic.ContainsKey(componentKeyAmmo.Description.Id))
                                        cDDic.Add(componentKeyAmmo.Description.Id, BattleEngineJsonCreation.ComponentDefType.AmmunitionBox);
                                    string[] splitAmmo = componentKeyAmmo.Description.UiName.Split(' ');

                                    if (splitAmmo.Length == 2)
                                    {
                                        if (splitAmmo[0].Contains("MG"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + "Machine Gun", (componentKeyAmmo.Description.Id, BattleEngineJsonCreation.ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("Gauss"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " Rifle", (componentKeyAmmo.Description.Id, BattleEngineJsonCreation.ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("SRM"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 2", (componentKeyAmmo.Description.Id, BattleEngineJsonCreation.ComponentDefType.AmmunitionBox));
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 4", (componentKeyAmmo.Description.Id, BattleEngineJsonCreation.ComponentDefType.AmmunitionBox));
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 6", (componentKeyAmmo.Description.Id, BattleEngineJsonCreation.ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("LRM"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 5", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 10", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 15", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0] + " 20", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[1].Contains("Ammo"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + splitAmmo[0], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("Ammo"))
                                        {
                                            CDDicT.Add(splitAmmo[0] + " " + splitAmmo[1], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        CDDicT.Add("Ammo" + " " + splitAmmo[0], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                        break;
                                    }
                                    if (splitAmmo.Length == 3)
                                    {
                                        if (splitAmmo[0].Contains("LB"))
                                        {
                                            CDDicT.Add(splitAmmo[2] + " " + splitAmmo[0] + " " + splitAmmo[1] + " AC", (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("MG"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + "Machine Gun: " + splitAmmo[2], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
                                            break;
                                        }
                                        if (splitAmmo[0].Contains("Half"))
                                        {
                                            CDDicT.Add(splitAmmo[1] + " " + "Machine Gun: " + splitAmmo[2], (componentKeyAmmo.Description.Id, ComponentDefType.AmmunitionBox));
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
                                    if (!cDDic.ContainsKey(componentKeyHeatSink.Description.Id))
                                        cDDic.Add(componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink);
                                    string[] splitHeatSink = componentKeyHeatSink.Description.UiName.Split(' ');
                                    if (splitHeatSink.Length == 2)
                                    {
                                        CDDicT.Add(splitHeatSink[0] + " " + splitHeatSink[1], (componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink));
                                        break;
                                    }
                                    if (splitHeatSink.Length == 3)
                                    {
                                        if (splitHeatSink[2].Contains("CD"))
                                        {
                                            CDDicT.Add("Clan Double Heat Sink", (componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink));
                                            break;
                                        }
                                        if (splitHeatSink[2].Contains("D"))
                                        {
                                            CDDicT.Add("Double Heat Sink", (componentKeyHeatSink.Description.Id, ComponentDefType.HeatSink));
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
                                    if (!cDDic.ContainsKey(componentKeyJumpjet.Description.Id))
                                        cDDic.Add(componentKeyJumpjet.Description.Id, ComponentDefType.JumpJet);
                                    break;
                                }
                            case "Upgrade":
                                {
                                    var componentKeyUpgrade = Upgrade.FromJson(jsonString);
                                    if (componentKeyUpgrade.Description == null) break;
                                    if (componentKeyUpgrade.Description.UiName == null) break;
                                    if (componentKeyUpgrade.Description.UiName.Contains("__")) break;
                                    if (!cDDic.ContainsKey(componentKeyUpgrade.Description.Id))
                                    {
                                        cDDic.Add(componentKeyUpgrade.Description.Id, ComponentDefType.Upgrade);
                                        CDDicT.Add(componentKeyUpgrade.Description.UiName, (componentKeyUpgrade.Description.Id, ComponentDefType.Upgrade));
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
                                    if (!cDDic.ContainsKey(componentKeyWeapon.Description.Id))
                                    {
                                        cDDic.Add(componentKeyWeapon.Description.Id, ComponentDefType.Weapon);
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
                                            CDDicT.Add(LRM, (componentKeyWeapon.Description.Id, ComponentDefType.Weapon));
                                            break;
                                        }
                                        if (componentKeyWeapon.Description.UiName.Contains("SRM"))
                                        {
                                            string LRM = componentKeyWeapon.Description.UiName.Replace("SRM", "SRM ");
                                            CDDicT.Add(LRM, (componentKeyWeapon.Description.Id, ComponentDefType.Weapon));
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
                                        CDDicT.Add(componentKeyWeapon.Description.UiName, (componentKeyWeapon.Description.Id, ComponentDefType.Weapon));
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
            String csv = String.Join(Environment.NewLine, cDDic.Select(d => $"{d.Key},{d.Value}"));
            return CDDicT;
        }
    }
}
