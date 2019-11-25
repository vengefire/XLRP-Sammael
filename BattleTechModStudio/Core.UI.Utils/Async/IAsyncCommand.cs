using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.UI.Utils.Async
{
    public interface IAsyncCommand : IAsyncCommand<object>
    {
    }

    public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged
    {
        ICommand Command { get; }

        bool CanExecute(object obj);

        Task ExecuteAsync(T obj);
    }
}