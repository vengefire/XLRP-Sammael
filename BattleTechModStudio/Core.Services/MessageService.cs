﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Core.Interfaces.Data;
using Core.Interfaces.Services;
using Core.Services.Annotations;

namespace Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly List<IMessageData> _messageData = new List<IMessageData>();

        public IEnumerable<string> Messages
        {
            get { return _messageData.Select(data => data.Message); }
        }

        public void AddMessage(IMessageData messageData)
        {
            _messageData.Add(messageData);
            OnPropertyChanged(nameof(MessageService.Messages));
        }

        public void ClearMessages()
        {
            this._messageData.Clear();
            OnPropertyChanged(nameof(Messages));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}