using System.Windows.Input;
using UI.Core.Interfaces.Services;

namespace BattleTechModStudio.Commands
{
    public static class CommonCommands
    {
        static CommonCommands()
        {
            SettingsService = App.Container.GetInstance<ISettingsService>();
            SaveCurrentSettingsCommand = new SaveSettingsCommand(SettingsService);
            LoadCurrentSettingsCommand = new LoadSettingsCommand(SettingsService);
        }

        public static ICommand LoadCurrentSettingsCommand { get; }

        public static ICommand SaveCurrentSettingsCommand { get; }

        public static ISettingsService SettingsService { get; }
    }
}