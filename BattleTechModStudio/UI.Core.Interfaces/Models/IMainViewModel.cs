using System.Collections.Generic;
using System.ComponentModel;
using UI.Core.Interfaces.Data;

namespace UI.Core.Interfaces.Models
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        bool IsBusy { get; }
        IEnumerable<string> Messages { get; }
        IPluginControl CurrentPluginControl { get; set; }
        void ClearMessages();
    }
}