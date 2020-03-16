using System.ComponentModel;

namespace BattleEngineParser.Enums
{
    public enum RulesLevel
    {
        [Description("Intro")] Intro,
        [Description("Standard")] Standard,
        [Description("Experimental")] Experimental,
        [Description("Advanced")] Advanced,
    }
}