using System.Collections.Generic;
using System.ComponentModel;
using Core.Interfaces.Data;

namespace Core.Interfaces.Services
{
    public interface IMessageService : INotifyPropertyChanged
    {
        IEnumerable<string> Messages { get; }
        void AddMessage(IMessageData messageData);
        void ClearMessages();
    }
}