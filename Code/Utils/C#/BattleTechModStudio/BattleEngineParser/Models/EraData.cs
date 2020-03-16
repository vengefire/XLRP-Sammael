using System.Collections.Generic;
using System.Linq;

namespace BattleEngineParser.Models
{
    public class EraData
    {
        public EraData(EraDetail eraDetail, List<string> factions)
        {
            EraDetail = eraDetail;
            if (factions != null && factions.Any())
            {
                Factions.AddRange(factions);
            }
        }

        public EraDetail EraDetail { get; set; }
        public List<string> Factions { get; set; } = new List<string>();
    }
}