using System.Windows;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Services.Logging.Log4netIntegration;
using Castle.Windsor;
using Core.Interfaces.Models;
using Core.Interfaces.Services;
using Core.Services;
using Framework.Interfaces.Logging;

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