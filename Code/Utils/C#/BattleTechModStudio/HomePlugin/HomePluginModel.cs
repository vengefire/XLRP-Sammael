using System.ComponentModel;
using System.Runtime.CompilerServices;
using HomePlugin.Annotations;

namespace HomePlugin
{
    public class HomePluginModel : INotifyPropertyChanged
    {
        private HomePluginSettings _settings;

        public HomePluginSettings Settings
        {
            get => _settings;
            set
            {
                if (Equals(value, _settings)) return;
                _settings = value;
                _settings.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}