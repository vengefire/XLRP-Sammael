using System.Collections.Generic;

namespace UI.Core.Interfaces.Data
{
    public interface IPluginCommandCategory
    {
        List<IPluginCommand> Commands { get; }

        string Name { get; }
    }
}