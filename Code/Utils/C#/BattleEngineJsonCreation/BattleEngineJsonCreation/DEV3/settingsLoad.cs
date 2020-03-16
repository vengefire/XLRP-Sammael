using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BattleEngineJsonCreation
{
    class settingsLoad
    {
        public static Settings LoadSettings()
        {
            string filepath = Directory.GetCurrentDirectory();
            
            string settingfile = Path.Combine(filepath, "settings.json");
            var fullPath = Path.Combine(filepath, settingfile);
            //var settings = new Settings();
            var settings = new Settings();

            if (File.Exists(fullPath))
            {
                string jsonString = File.ReadAllText(settingfile);
                settings = Settings.FromJson(jsonString);
            }
            else
            {
                File.WriteAllText(fullPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
                Reuse.EndProgram("Settings file not found, created new default.");
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
