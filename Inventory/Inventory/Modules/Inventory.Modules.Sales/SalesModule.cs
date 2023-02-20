using Inventory.Modules.Sales.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Inventory.Modules.Sales
{
    public class SalesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SalesView>();
        }
    }
}