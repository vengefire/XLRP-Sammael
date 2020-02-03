using System;
using System.IO;
using UI.Core.Interfaces.Services;
using Newtonsoft.Json;

namespace UI.Core.Services
{
    public class SettingsService : ISettingsService
    {
        public TType ReadSettings<TType>(string name)
        {
            if (!File.Exists(SettingsFileName("./", name)))
            {
                return Activator.CreateInstance<TType>();
            }

            return JsonConvert.DeserializeObject<TType>(File.ReadAllText($"./{name}.json"));
        }

        public object ReadSettings(string name, Type settingsType)
        {
            if (!File.Exists(SettingsFileName("./", name)))
            {
                return Activator.CreateInstance(settingsType);
            }

            return JsonConvert.DeserializeObject(File.ReadAllText($"./{name}.json"), settingsType);
        }

        public void SaveSettings(string name, object settings)
        {
            File.WriteAllText(
                $"./{name}.json",
                JsonConvert.SerializeObject(
                    settings,
                    Formatting.Indented,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
        }

        private string SettingsFileName(string path, string name)
        {
            return Path.Combine(path, $"{name}.json");
        }
    }
}