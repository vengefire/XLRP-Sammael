using System.Windows.Input;
using UI.Core.Interfaces.Services;

namespace BattleTechModStudio.Commands
{
    public static class CommonCommands
    {
        static CommonCommands()
        {
            CommonCommands.SettingsService = App.Container.GetInstance<ISettingsService>();
            CommonCommands.SaveCurrentSettingsCommand = new SaveSettingsCommand(CommonCommands.SettingsService);
            CommonCommands.LoadCurrentSettingsCommand = new LoadSettingsCommand(CommonCommands.SettingsService);
        }

        public static ICommand LoadCurrentSettingsCommand { get; }

        public static ICommand SaveCurrentSettingsCommand { get; }

        public static ISettingsService SettingsService { get; }
    }
}