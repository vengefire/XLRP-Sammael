using System.ComponentModel;
using UI.Core.Interfaces.Services;

namespace UI.Core.Interfaces.Models
{
    public interface IMainModel : INotifyPropertyChanged
    {
        bool IsBusy { get; set; }
        IMessageService MessageService { get; }
    }
}