using System.Collections.Generic;
using System.ComponentModel;
using UI.Core.Interfaces.Data;

namespace UI.Core.Interfaces.Services
{
    public interface IMessageService : INotifyPropertyChanged
    {
        IEnumerable<string> Messages { get; }
        void AddMessage(IMessageData messageData);
        void ClearMessages();
    }
}