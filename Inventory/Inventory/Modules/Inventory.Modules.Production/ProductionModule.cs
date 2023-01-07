using Inventory.Modules.Production.Views;
using Inventory.Modules.Production.Views.CategoryDialogs;
using Prism.Ioc;
using Prism.Modularity;

namespace Inventory.Modules.Production
{
    public class ProductionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CategoriesView>();
            containerRegistry.RegisterForNavigation<CatalogView>();
            containerRegistry.RegisterForNavigation<CategoriesFormDialogView>();
        }
    }
}