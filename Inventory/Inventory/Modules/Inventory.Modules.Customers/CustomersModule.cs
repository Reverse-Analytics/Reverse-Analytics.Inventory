using Inventory.Modules.Customers.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Inventory.Modules.Customers
{
    public class CustomersModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CustomersView>();
        }
    }
}