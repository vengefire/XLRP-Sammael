using System;
using System.Collections.Generic;

namespace UI.Core.Interfaces.Data
{
    public interface IPluginControl
    {
        string ModuleName { get; }

        List<IPluginCommand> PluginCommands { get; }

        object Settings { get; set; }

        Type SettingsType { get; }
    }
}