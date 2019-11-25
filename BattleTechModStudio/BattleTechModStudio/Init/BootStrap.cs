using Framework.Interfaces.Injection;

namespace BattleTechModStudio.Init
{
    internal class Bootstrap : IBootstrap
    {
        public IContainer RegisterContainer()
        {
            IContainer container = new CastleWindsorContainer();
            Container.RegisterContainer(container);
            return container;
        }
    }
}