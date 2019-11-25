using System.Windows.Input;

namespace Core.Interfaces.Data
{
    public interface IPluginCommand : ICommand
    {
        IPluginCommandCategory Category { get; }

        object CommandParameter { get; }

        string Name { get; }
    }
}