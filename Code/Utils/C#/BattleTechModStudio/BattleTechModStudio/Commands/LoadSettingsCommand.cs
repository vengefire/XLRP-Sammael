﻿using System;
using System.Windows.Input;
using UI.Core.Interfaces.Data;
using UI.Core.Interfaces.Services;

namespace BattleTechModStudio.Commands
{
    public class LoadSettingsCommand : ICommand
    {
        private readonly ISettingsService _settingsService;

        public LoadSettingsCommand(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IPluginCommandCategory Category { get; }

        public string Name => @"Load Settings";

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var plugin = parameter as IPluginControl;
            plugin.Settings = _settingsService.ReadSettings(plugin.ModuleName, plugin.SettingsType);
        }
    }
}