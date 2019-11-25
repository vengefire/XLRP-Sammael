using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Interfaces.Data;
using Core.Interfaces.Models;
using Core.Services.Annotations;

namespace BattleTechModStudio
{
    public class MainViewModel : IMainViewModel
    {
        public MainViewModel(IMainModel mainModel)
        {
            MainModel = mainModel;
            MainModel.PropertyChanged += (sender, args) => { OnPropertyChanged(args.PropertyName); };
        }

        public IMainModel MainModel { get; }

        public bool IsBusy => MainModel.IsBusy;

        public IEnumerable<string> Messages => MainModel.MessageService.Messages;
        public IPluginControl CurrentPluginControl { get; set; }

        public void ClearMessages()
        {
            MainModel.MessageService.ClearMessages();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}