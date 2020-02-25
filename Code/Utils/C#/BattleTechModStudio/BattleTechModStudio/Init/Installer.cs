using System.Windows;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Services.Logging.Log4netIntegration;
using Castle.Windsor;
using Framework.Interfaces.Logging;
using UI.Core.Interfaces.Models;
using UI.Core.Interfaces.Services;
using UI.Core.Services;

namespace BattleTechModStudio.Init
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Window>().ImplementedBy<MainWindow>(),
                Component.For<ILogger>().ImplementedBy<Log4netLogger>(),
                Component.For<IExceptionLogger>().ImplementedBy<ExceptionLogger>(),
                Component.For<IMessageService>().ImplementedBy<MessageService>().LifestyleSingleton(),
                Component.For<ISettingsService>().ImplementedBy<SettingsService>().LifestyleSingleton(),
                Component.For<IMainModel>().ImplementedBy<MainModel>().LifestyleSingleton(),
                Component.For<IMainViewModel>().ImplementedBy<MainViewModel>().LifestyleSingleton()
            );
        }
    }
}