using Inventory.Modules.Dashboard.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Inventory.Modules.Dashboard
{
    public class DashboardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DashboardView>();
        }
    }
}