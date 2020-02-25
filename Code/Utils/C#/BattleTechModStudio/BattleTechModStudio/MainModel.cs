using System.ComponentModel;
using System.Runtime.CompilerServices;
using UI.Core.Data;
using UI.Core.Interfaces.Models;
using UI.Core.Interfaces.Services;
using UI.Core.Services.Annotations;

namespace BattleTechModStudio
{
    public class MainModel : INotifyPropertyChanged, IMainModel
    {
        public MainModel(IMessageService messageService)
        {
            MessageService = messageService;
            MessageService.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
            MessageService.AddMessage(new MessageData
            {
                Message = "Main Model has been Initialized."
            });
        }

        public bool IsBusy { get; set; }

        public IMessageService MessageService { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}