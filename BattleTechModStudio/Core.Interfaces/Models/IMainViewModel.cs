using System.Collections.Generic;
using System.ComponentModel;
using Core.Interfaces.Data;

namespace Core.Interfaces.Models
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        bool IsBusy { get; }
        IEnumerable<string> Messages { get; }
        IPluginControl CurrentPluginControl { get; set; }
        void ClearMessages();
    }
}