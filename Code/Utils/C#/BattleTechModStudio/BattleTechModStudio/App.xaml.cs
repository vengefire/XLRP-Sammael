using System.Windows;
using BattleTechModStudio.Init;
using Framework.Interfaces.Injection;

namespace BattleTechModStudio
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var bootstrapper = new Bootstrap();
            Container = bootstrapper.RegisterContainer();
            var mainWindow = Container.GetInstance<Window>();
            mainWindow.Show();
        }
    }
}