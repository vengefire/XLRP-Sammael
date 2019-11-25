using System.ComponentModel;
using Core.Interfaces.Services;

namespace Core.Interfaces.Models
{
    public interface IMainModel : INotifyPropertyChanged
    {
        bool IsBusy { get; set; }
        IMessageService MessageService { get; }
    }
}