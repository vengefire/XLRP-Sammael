using System;
using System.IO;

namespace BattleEngineJsonCreation
{
    public static class chassisHelper
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
            mechdef.Description.Name = chassisDef.Description.Icon;

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
    }
}
