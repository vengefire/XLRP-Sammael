using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation
{
    public class LoadSettings
    {
        static string filepath = Directory.GetCurrentDirectory();
        //Settings Processing and Loading
        static string settingfile = Path.Combine(filepath, "settings.json");

    }

    class Loader
    {
    }
}
