using System;
using System.Collections.Generic;

namespace Core.Interfaces.Data
{
    public interface IPluginControl
    {
        string ModuleName { get; }

        List<IPluginCommand> PluginCommands { get; }

        object Settings { get; set; }

        Type SettingsType { get; }
    }
}