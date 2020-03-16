using BattleEngineParser.Enums;

namespace BattleEngineParser.Models
{
    public class EraDetail
    {
        public EraDetail(Era era, string eraYearRange)
        {
            Era = era;
            EraYearRange = eraYearRange;
        }

        public Era Era { get; }
        public string EraYearRange { get; }
    }
}