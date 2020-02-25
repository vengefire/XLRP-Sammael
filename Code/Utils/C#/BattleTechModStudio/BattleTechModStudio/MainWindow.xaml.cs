using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BattleTechModStudio.Commands;
using Framework.Interfaces.Injection;
using UI.Core.Interfaces.Data;
using UI.Core.Interfaces.Models;
using UI.Core.Services;

namespace BattleTechModStudio
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<IPlugin> _plugins;
        private PluginService _pluginService;

        public MainWindow(IMainViewModel mainViewModel)
        {
            InitializeComponent();
            InitializePlugins();
            MainViewModel = mainViewModel;
            DataContext = MainViewModel;
        }

        public IMainViewModel MainViewModel { get; }

        private void InitializePlugins()
        {
            _pluginService = new PluginService();
            _plugins = _pluginService.GetPlugins(".");
            foreach (var plugin in _plugins)
            {
                var moduleTab = new TabItem {Header = plugin.Name};
                var modulePage = Container.Instance.GetInstance(plugin.PageType);
                moduleTab.Content = modulePage;
                TabPages.Items.Add(moduleTab);
            }
        }

        // TODO: this needs to go into a single activation spot
        private void TabPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;

            // Every Tab Control in the entire app, I tell you... ffs.
            if (tabControl == null || tabControl.Name != "TabPages")
            {
                return;
            }

            // Remove non-common tool bars...
            var toolbars = ToolbarTray.ToolBars.Where(bar => bar.Name != "TbCommon").ToList();
            toolbars.ForEach(bar => ToolbarTray.ToolBars.Remove(bar));

            var tabItem = (sender as TabControl).SelectedItem as TabItem;
            if (!(tabItem?.Content is IPluginControl))
            {
                return;
            }

            var pluginModule = (IPluginControl) tabItem.Content;
            var pluginToolbar = new ToolBar();

            foreach (var command in pluginModule.PluginCommands)
            {
                pluginToolbar.Items.Add(
                    new Button
                    {
                        Content = command.Name,
                        Command = command,
                        CommandParameter = command.CommandParameter
                    });
            }

            ToolbarTray.ToolBars.Add(pluginToolbar);

            // Init settings and commands...
            MainViewModel.CurrentPluginControl = pluginModule;
            if (pluginModule.Settings == null)
            {
                CommonCommands.LoadCurrentSettingsCommand.Execute(pluginModule);
            }
        }

        private void ClearMessages_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel.ClearMessages();
        }
    }
}