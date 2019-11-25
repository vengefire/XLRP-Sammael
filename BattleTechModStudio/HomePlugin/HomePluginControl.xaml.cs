using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Core.Interfaces.Data;

namespace HomePlugin
{
    /// <summary>
    ///     Interaction logic for HomePluginControl.xaml
    /// </summary>
    public partial class HomePluginControl : UserControl, IPluginControl
    {
        public HomePluginViewModel HomePluginViewModel { get; }

        public HomePluginControl()
        {
            HomePluginViewModel = new HomePluginViewModel(new HomePluginModel());
            InitializeComponent();
        }

        public string ModuleName => @"Battle Tech Mod Studio - Home";
        public List<IPluginCommand> PluginCommands { get; }

        public object Settings
        {
            get => HomePluginViewModel.HomePluginSettings; 
            set => HomePluginViewModel.HomePluginSettings = value as HomePluginSettings;
        }
        public Type SettingsType => typeof(HomePluginSettings);
    }
}