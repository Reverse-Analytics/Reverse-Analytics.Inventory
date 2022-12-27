using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Inventory.Modules.Regions.ViewModels
{
    public class SidebarViewModel : BindableBase
    {
        private Visibility _catalogVisibility = Visibility.Collapsed;
        public Visibility CatalogVisibility 
        {
            get => _catalogVisibility;
            set => SetProperty(ref _catalogVisibility, value);
        }
        private Visibility _categoriesVisibility = Visibility.Collapsed;
        public Visibility CategoriesVisibility 
        {
            get => _categoriesVisibility;
            set => SetProperty(ref _categoriesVisibility, value);
        }

        private Visibility _productionChildrenVisibility = Visibility.Collapsed;
        public Visibility ProductionChildrenVisibility
        {
            get => _productionChildrenVisibility;
            set => SetProperty(ref _productionChildrenVisibility, value);
        }
        public Visibility CustomerDebtsVisibility { get; set; }
        public Visibility SuppliesVisibility { get; set; }
        public Visibility SupplierDebtsVisibility { get; set; }

        public ICommand ProductionsCommand { get; set; }


        public bool _isDashboardButtonActive = false;
        public bool IsDashboardButtonActive
        {
            get => _isDashboardButtonActive;
            set => SetProperty(ref _isDashboardButtonActive, value);
        }

        public bool _isProductionButtonActive = false;
        public bool IsProductionButtonActive 
        {
            get => _isProductionButtonActive;
            set
            {
                SetProperty(ref _isProductionButtonActive, value);
                if (value)
                {
                    ProductionChildrenVisibility = Visibility.Visible;
                }
                else
                {
                    ProductionChildrenVisibility = Visibility.Collapsed;
                }
            }
        }

        public SidebarViewModel()
        {
            CatalogVisibility = Visibility.Collapsed;
            CategoriesVisibility = Visibility.Collapsed;
            CustomerDebtsVisibility = Visibility.Collapsed;
            CustomerDebtsVisibility = Visibility.Collapsed;
            SuppliesVisibility = Visibility.Collapsed;
            SupplierDebtsVisibility = Visibility.Collapsed;

            ProductionsCommand = new DelegateCommand(OnProductionsCommand);
        }

        private void OnProductionsCommand()
        {
            IsProductionButtonActive = !IsProductionButtonActive;
        }
    }
}
