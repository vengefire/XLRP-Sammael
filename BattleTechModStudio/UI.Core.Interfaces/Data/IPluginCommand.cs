using System.Windows.Input;

namespace UI.Core.Interfaces.Data
{
    public interface IPluginCommand : ICommand
    {
        IPluginCommandCategory Category { get; }

        object CommandParameter { get; }

        string Name { get; }
    }
}