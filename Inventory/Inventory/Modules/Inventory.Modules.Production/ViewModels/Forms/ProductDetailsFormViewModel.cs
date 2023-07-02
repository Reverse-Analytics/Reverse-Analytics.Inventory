using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;

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

        public DelegateCommand CloseCommand { get; }

        public ProductDetailsFormViewModel(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            Id = product.Id;
            ProductName = product.ProductName;
            ProductCode = product.ProductCode;
            CategoryName = product?.Category?.CategoryName;
            NumberOfItems = product.Id;
            Volume = product.Volume ?? 0;
            Weight = product.Weight ?? 0;
            IncomePrice = product.SupplyPrice ?? 0;
            SalePrice = product.SalePrice;

            CloseCommand = new DelegateCommand(OnClose);
        }

        private void OnClose() => DialogHost.Close(RegionNames.DialogRegion);
    }
}
