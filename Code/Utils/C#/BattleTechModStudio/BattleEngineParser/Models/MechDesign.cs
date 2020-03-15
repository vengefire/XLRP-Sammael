using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleEngineParser.Enums;
using Data.Utils.Extensions;

namespace BattleEngineParser.Models
{
    public class MechDesign
    {
        public string Name { get; set; }
        public TechLevel TechLevel { get; set; }
        public RulesLevel RulesLevel { get; set; }
        public ChassisType ChassisType { get; set; }
        public double BattleValue { get; set; }
        public int Tonnage { get; set; }
        public int Year { get; set; }
        public List<EraData> Eras { get; set; } = new List<EraData>();
        public string EngineRating { get; set; }
        public string HeatSinks { get; set; }
        public string StructureType { get; set; }
        public string ArmorType { get; set; }
        public bool IsOmni { get; set; }
        
        public static MechDesign MechDesignFromFile(string filePath)
        {
            var mechDesign = new MechDesign();
            using (var fileStream = new StreamReader(filePath))
            {
                string line;
                while ((line = fileStream.ReadLine()) != null)
                {
                    line = line.Replace("\"", "");
                    var parts = line.Split(',');
                    switch (parts[0])
                    {
                        case "Name":
                            mechDesign.Name = parts[1];
                            break;
                        case "Tech":
                            mechDesign.TechLevel = parts[1].FromEnumStringValue<TechLevel>();
                            break;
                        case "Rules":
                            if (parts.Length == 2)
                            {
                                mechDesign.RulesLevel = parts[1].FromEnumStringValue<RulesLevel>();
                            }

                            break;
                        case "Chassis":
                            mechDesign.ChassisType = parts[1].FromEnumStringValue<ChassisType>();
                            break;
                        case "Tons":
                            mechDesign.Tonnage = Convert.ToInt32(parts[1]);
                            break;
                        case "Year":
                            mechDesign.Year = string.IsNullOrEmpty(parts[1]) ? 0 : Convert.ToInt32(parts[1]);
                            break;
                        case "Era":
                            var era = new EraDetail(parts[1].FromEnumStringValue<Era>(), parts[2]);
                            var eraData = new EraData(era, null);
                            var factions = fileStream.ReadLine().Replace("\"", "").Split(',').ToList();
                            factions.Skip(1).ToList().ForEach(s => eraData.Factions.Add(s));
                            mechDesign.Eras.Add(eraData);
                            break;
                        case "Omni":
                            mechDesign.IsOmni = parts[1] != "0";
                            break;
                    }
                }
            }
            return mechDesign;
        }
    }
}