using Inventory.Core;
using Inventory.Modules.Regions.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Inventory.Modules.Regions
{
    public class RegionsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public RegionsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.MenuRegion, "MenuView");
            _regionManager.RequestNavigate(RegionNames.NavigationRegion, "SidebarView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MenuView>();
            containerRegistry.RegisterForNavigation<SidebarView>();
        }
    }
}