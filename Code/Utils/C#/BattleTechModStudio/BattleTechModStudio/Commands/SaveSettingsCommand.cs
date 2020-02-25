using System;
using System.Windows.Input;
using UI.Core.Interfaces.Data;
using UI.Core.Interfaces.Services;

namespace BattleTechModStudio.Commands
{
    public class SaveSettingsCommand : ICommand
    {
        private readonly ISettingsService _settingsService;

        public SaveSettingsCommand(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IPluginCommandCategory Category { get; }

        public string Name => @"Save Settings";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var plugin = parameter as IPluginControl;
            _settingsService.SaveSettings(plugin.ModuleName, plugin.Settings);
        }
    }
}