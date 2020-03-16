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
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                var parts = value.Replace("_", " ").Split(' ').ToList();
                if (parts[0].Contains("-"))
                {
                    // Standard IS mech. Smart!
                    VariantDesignation = parts[0];
                    parts.Skip(1).ToList().ForEach(s =>
                    {
                        if (s.Contains("("))
                        {
                            s = s.Replace("(", "");
                            s = s.Replace(")", "");
                            HeroDesignation.Add(s);
                        }
                        else if (s.Contains("'"))
                        {
                            s = s.Replace("'", "");
                            HeroDesignation.Add(s);
                        }
                        else
                        {
                            InnerSphereChassisDesignation = s;
                        }
                    });
                }
                else
                {
                    // Clan Mech. Ugh, crapy naming system.
                    InnerSphereChassisDesignation = parts[0];
                    List<string> remainingParts;
                    if (parts[1].Contains("("))
                    {
                        FilthyClanChassisDesignation = parts[1].Replace("(", "").Replace(")", "");
                        remainingParts = parts.Skip(2).ToList();
                    }
                    else
                    {
                        FilthyClanChassisDesignation = InnerSphereChassisDesignation;
                        remainingParts = parts.Skip(1).ToList();
                    }

                    remainingParts.ForEach(s =>
                    {
                        if (s.Contains("("))
                        {
                            s = s.Replace("(", "");
                            s = s.Replace(")", "");
                            HeroDesignation.Add(s);
                        }
                        else if (s.Contains("'"))
                        {
                            s = s.Replace("'", "");
                            HeroDesignation.Add(s);
                        }
                        else
                        {
                            VariantDesignation = s;
                        }
                    });
                }
            }
        }

        public string InnerSphereChassisDesignation { get; set; }
        public string FilthyClanChassisDesignation { get; set; }
        public string VariantDesignation { get; set; }
        public List<string> HeroDesignation { get; set; } = new List<string>();
        public TechLevel TechLevel { get; set; }
        public RulesLevel RulesLevel { get; set; }
        public ChassisType ChassisType { get; set; }
        public double BattleValue { get; set; }
        public int Tonnage { get; set; }
        public int? Year { get; set; }
        public List<EraData> Eras { get; set; } = new List<EraData>();
        public string EngineRating { get; set; }
        public string HeatSinks { get; set; }
        public string StructureType { get; set; }
        public string ArmorType { get; set; }
        public bool IsOmni { get; set; }
        public FileInfo FileInfo { get; set; }

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
                            mechDesign.Year = string.IsNullOrEmpty(parts[1]) ? (int?) null : Convert.ToInt32(parts[1]);
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

            mechDesign.FileInfo = new FileInfo(filePath);
            return mechDesign;
        }
    }
}