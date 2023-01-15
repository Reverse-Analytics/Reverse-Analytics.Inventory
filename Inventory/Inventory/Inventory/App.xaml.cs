using Inventory.Modules.Dashboard;
using Inventory.Modules.Dialogs;
using Inventory.Modules.Production;
using Inventory.Modules.Regions;
using Inventory.RestClient;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Inventory.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace Inventory
{
    public partial class App
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("ODE1Mjg2QDMyMzAyZTM0MmUzMGIyekp1VGJuc1RXK2ovWUJHK3V2RStZVFR0WkU4M1Q1a2JjRGs2VzcyWVE9");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterScoped<ICategorySerivce, CategoryService>();
            containerRegistry.RegisterScoped<IProductService, ProductService>();
            containerRegistry.RegisterSingleton<RestClientBase>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<RegionsModule>();
            moduleCatalog.AddModule<DashboardModule>();
            moduleCatalog.AddModule<ProductionModule>();
            moduleCatalog.AddModule<DialogsModule>();
        }
    }
}
