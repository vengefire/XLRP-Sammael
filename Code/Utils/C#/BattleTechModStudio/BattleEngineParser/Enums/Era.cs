using System.ComponentModel;

namespace BattleEngineParser.Enums
{
    public enum Era
    {
        [Description("Early Succession War")] EarlySuccessionWar,

        [Description("Late Succession War - LosTech")]
        LateSuccessionWarLosTech,

        [Description("Late Succession War - Renaissance")]
        LateSuccessionWarRenaissance,
        [Description("Clan Invasion")] Clan_Invasion,
        [Description("Civil War")] Civil_War,
        [Description("Jihad")] Jihad,
        [Description("Early Republic")] Early_Republic,
        [Description("Star League")] Star_League,
        [Description("Late Republic")] Late_Republic,
        [Description("Dark Ages")] Dark_Ages,
    }
}