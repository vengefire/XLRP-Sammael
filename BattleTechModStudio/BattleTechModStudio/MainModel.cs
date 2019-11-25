using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Data;
using Core.Interfaces.Models;
using Core.Interfaces.Services;
using Core.Services.Annotations;

namespace BattleTechModStudio
{
    public class MainModel : INotifyPropertyChanged, IMainModel
    {
        public bool IsBusy { get; set;  }

        public MainModel(IMessageService messageService)
        {
            MessageService = messageService;
            MessageService.PropertyChanged += (sender, args) => this.OnPropertyChanged(args.PropertyName);
            MessageService.AddMessage(new MessageData()
            {
                Message = "Main Model has been Initialized."
            });
        }

        public IMessageService MessageService { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}