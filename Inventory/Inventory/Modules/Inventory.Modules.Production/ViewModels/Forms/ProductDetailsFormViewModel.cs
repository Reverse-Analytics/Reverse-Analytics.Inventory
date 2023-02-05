using Inventory.Core;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using ReverseAnalytics.Domain.DTOs.Product;

namespace Inventory.Modules.Production.ViewModels.Forms
{
    public class ProductDetailsFormViewModel : ViewModelBase
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public int NumberOfItems { get; set; }
        public double Volume { get; set; }
        public double Weight { get; set; }
        public decimal IncomePrice { get; set; }
        public decimal SalePrice { get; set; }

        public ProductDto Product { get; set; }

        public DelegateCommand CloseCommand { get; }

        public ProductDetailsFormViewModel(ProductDto product)
        {
            Id = product.Id;
            ProductName = product.ProductName;
            ProductCode = product.ProductCode;
            CategoryName = product.Category.CategoryName;
            NumberOfItems = product.Id;
            Volume = product.Volume;
            Weight = product.Weight;
            IncomePrice = product.SupplyPrice;
            SalePrice = product.SupplyPrice;

            CloseCommand = new DelegateCommand(OnClose);
        }

        private void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
