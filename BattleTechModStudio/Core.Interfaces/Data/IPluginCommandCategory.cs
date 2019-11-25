using System.Collections.Generic;

namespace Core.Interfaces.Data
{
    public interface IPluginCommandCategory
    {
        List<IPluginCommand> Commands { get; }

        string Name { get; }
    }
}