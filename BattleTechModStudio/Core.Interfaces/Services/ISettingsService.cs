using System;

namespace Core.Interfaces.Services
{
    public interface ISettingsService
    {
        TType ReadSettings<TType>(string name);

        object ReadSettings(string name, Type settingsType);

        void SaveSettings(string name, object settings);
    }
}