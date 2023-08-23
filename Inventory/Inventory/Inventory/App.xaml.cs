using Inventory.Modules.Customers;
using Inventory.Modules.Dashboard;
using Inventory.Modules.Dialogs;
using Inventory.Modules.Production;
using Inventory.Modules.Regions;
using Inventory.Modules.Sales;
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
            Syncfusion
                .Licensing
                .SyncfusionLicenseProvider
                .RegisterLicense("MjYzNjQxMEAzMjMyMmUzMDJlMzBOc3R2Z0tvMUZkQ2Fidnlwck1ZbTZKSFhJV1kvMTYveklYTnBlSTNlU0FBPQ==");
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterScoped<ICategorySerivce, CategoryService>();
            containerRegistry.RegisterScoped<IProductService, ProductService>();
            containerRegistry.RegisterScoped<ICustomerService, CustomerService>();
            containerRegistry.RegisterScoped<ISaleService, SaleService>();
            containerRegistry.RegisterScoped<ISaleDebtService, SaleDebtService>();
            containerRegistry.RegisterSingleton<RestClientBase>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<RegionsModule>();
            moduleCatalog.AddModule<DashboardModule>();
            moduleCatalog.AddModule<ProductionModule>();
            moduleCatalog.AddModule<CustomersModule>();
            moduleCatalog.AddModule<DialogsModule>();
            moduleCatalog.AddModule<SalesModule>();
        }
    }
}