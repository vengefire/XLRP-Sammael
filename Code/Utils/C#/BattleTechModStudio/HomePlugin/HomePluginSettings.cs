using System.ComponentModel;
using System.Runtime.CompilerServices;
using HomePlugin.Annotations;

namespace HomePlugin
{
    public class HomePluginSettings : INotifyPropertyChanged
    {
        private string _battleTechDirectory;
        private string _battleTechDlcDirectory;
        private string _modCollectionName;
        private string _modDirectory;
        private string _modTargetDirectory;

        public string ModTargetDirectory
        {
            get => _modTargetDirectory;
            set
            {
                if (value == _modTargetDirectory)
                {
                    return;
                }

                _modTargetDirectory = value;
                OnPropertyChanged();
            }
        }

        public string ModCollectionName
        {
            get => _modCollectionName;
            set
            {
                if (value == _modCollectionName)
                {
                    return;
                }

                _modCollectionName = value;
                OnPropertyChanged();
            }
        }

        public string ModDirectory
        {
            get => _modDirectory;
            set
            {
                if (value == _modDirectory)
                {
                    return;
                }

                _modDirectory = value;
                OnPropertyChanged();
            }
        }

        public string BattleTechDlcDirectory
        {
            get => _battleTechDlcDirectory;
            set
            {
                if (value == _battleTechDlcDirectory)
                {
                    return;
                }

                _battleTechDlcDirectory = value;
                OnPropertyChanged();
            }
        }

        public string BattleTechDirectory
        {
            get => _battleTechDirectory;
            set
            {
                if (value == _battleTechDirectory)
                {
                    return;
                }

                _battleTechDirectory = value;
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