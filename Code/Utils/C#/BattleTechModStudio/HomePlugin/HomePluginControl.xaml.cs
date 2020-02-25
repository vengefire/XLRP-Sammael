using System;
using System.Collections.Generic;
using System.Windows.Controls;
using UI.Core.Interfaces.Data;

namespace HomePlugin
{
    /// <summary>
    ///     Interaction logic for HomePluginControl.xaml
    /// </summary>
    public partial class HomePluginControl : UserControl, IPluginControl
    {
        public HomePluginControl()
        {
            HomePluginViewModel = new HomePluginViewModel(new HomePluginModel());
            InitializeComponent();
            DataContext = HomePluginViewModel;
            PluginCommands = new List<IPluginCommand>();
        }

        public HomePluginViewModel HomePluginViewModel { get; }

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