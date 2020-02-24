using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleEngineJsonCreation.DEV3
{
    class settingsLoad
    {
        public static Settings LoadSettings()
        {
            string filepath = Directory.GetCurrentDirectory();
            string settingfile = Path.Combine(filepath, "settings.json");
            //var settings = new Settings();
            var settings = new Settings();

            if (File.Exists(Path.Combine(filepath, settingfile)))
            {
                string jsonString = File.ReadAllText(settingfile);
                settings = Settings.FromJson(jsonString);
            }
            else
            {
                Reuse.EndProgram("Settings file not found!");
            }
            if (!Directory.Exists(settings.BtInstallDir)) Reuse.EndProgram("BT Direcotry not found at " + settings.BtInstallDir);
            if (!Directory.Exists(settings.BedPath)) Reuse.EndProgram("BED Files not found at " + settings.BedPath);
            //if (!Directory.Exists(settings.ChassisDefpath)) Reuse.EndProgram("Chassis Files not found at " + settings.ChassisDefpath);
            try
            {
                if (!Directory.Exists(settings.OutputDir)) Directory.CreateDirectory(settings.OutputDir);
            }
            catch (Exception e)
            {
                Reuse.EndProgram("Error Creating Directory: " + settings.OutputDir + " " + e);
            }
            return (settings);
        }
    }
}
