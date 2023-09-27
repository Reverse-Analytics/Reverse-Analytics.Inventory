using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inventory.Modules.Sales.ViewModels.Forms
{
    internal class SaleRefundFormViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public List<Sale> Sales { get; private set; }
        private List<Core.Models.SaleDetail> saleDetails;
        public ObservableCollection<Product> RefundableProducts { get; }
        public ObservableCollection<BindableRefundDetail> DetailsToRefund { get; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand<BindableRefundDetail> RemoveProductCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private Sale _selectedSale;
        public Sale SelectedSale
        {
            get => _selectedSale;
            set
            {
                if (value is null)
                {
                    return;
                }

                SetProperty(ref _selectedSale, value);
                SetupCollections(value);
                SetupSaleDebt(value);
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private decimal _refundTotalAmount;
        public decimal RefundTotalAmount
        {
            get => _refundTotalAmount;
            set => SetProperty(ref _refundTotalAmount, value);
        }

        private decimal _amountToRefund;
        public decimal AmountToRefund
        {
            get => _amountToRefund;
            set => SetProperty(ref _amountToRefund, value);
        }

        private decimal _initialDebtAmount;
        public decimal InitialDebtAmount
        {
            get => _initialDebtAmount;
            set => SetProperty(ref _initialDebtAmount, value);
        }

        private decimal _debtAmount;
        public decimal DebtAmount
        {
            get => _debtAmount;
            set
            {
                if (value < 0)
                {
                    SetProperty(ref _debtAmount, 0);
                    return;
                }

                if (value > _initialDebtAmount)
                {
                    SetProperty(ref _debtAmount, _initialDebtAmount);
                    return;
                }

                SetProperty(ref _debtAmount, value);
            }
        }

        private bool _isSaleSelectionEnabled;
        public bool IsSaleSelectionEnabled
        {
            get => _isSaleSelectionEnabled;
            set => SetProperty(ref _isSaleSelectionEnabled, value);
        }

        public SaleRefundFormViewModel(IDialogService dialogService, List<Sale> sales)
        {
            saleDetails = new List<Core.Models.SaleDetail>();
            RefundableProducts = new ObservableCollection<Product>();
            DetailsToRefund = new ObservableCollection<BindableRefundDetail>();

            _dialogService = dialogService;

            CancelCommand = new DelegateCommand(OnCancel);
            AddProductCommand = new DelegateCommand(OnAddProduct);
            RemoveProductCommand = new DelegateCommand<BindableRefundDetail>(OnDeleteProduct);
            Sales = sales;
            IsSaleSelectionEnabled = true;
        }

        public SaleRefundFormViewModel(IDialogService dialogService, Sale sale, List<Sale> sales)
            : this(dialogService, sales)
        {
            SetupCollections(sale);
            SetupSaleDebt(sale);

            SelectedSale = sale;
            IsSaleSelectionEnabled = false;
        }

        public void SetupCollections(Sale sale)
        {
            if (sale == null || sale.SaleDetails == null)
            {
                return;
            }

            var refundableProducts = sale.SaleDetails
                .Select(sd => sd.Product)
                .Distinct()
                .ToList();

            saleDetails.Clear();
            saleDetails.AddRange(sale.SaleDetails);

            RefundableProducts.Clear();
            RefundableProducts.AddRange(refundableProducts);
        }

        private void SetupSaleDebt(Sale sale)
        {
            if (sale.SaleDebt == null)
            {
                return;
            }

            InitialDebtAmount = sale.SaleDebt.TotalDue - sale.SaleDebt.TotalPaid;
            DebtAmount = sale.SaleDebt.TotalDue - sale.SaleDebt.TotalPaid;
        }

        public void OnAddProduct()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            var saleDetailToRefund = saleDetails.FirstOrDefault(x => x.Product.Id == SelectedProduct.Id);

            if (saleDetailToRefund is null || DetailsToRefund.Any(x => x.ProductId == saleDetailToRefund.ProductId))
            {
                return;
            }

            var saleDetailToAdd = new BindableRefundDetail
            {
                UnitPrice = saleDetailToRefund.UnitPrice,
                ProductCode = SelectedProduct.ProductCode,
                SaleQuantity = saleDetailToRefund.Quantity,
                ProductId = saleDetailToRefund.ProductId,
            };

            saleDetailToAdd.PropertyChanged += OnRefundDetailChanged;

            DetailsToRefund.Add(saleDetailToAdd);
        }

        public async void OnCancel()
        {
            var result = await _dialogService.ShowConfirmation("Please, confirm the action.", "Are you sure you want to close the dialog?");

            if (result)
            {
                DialogHost.Close(RegionNames.DialogRegion);
            }
        }

        public void OnDeleteProduct(BindableRefundDetail product)
        {
            var productToRemove = DetailsToRefund.FirstOrDefault(x => x.ProductId == product.ProductId);

            if (productToRemove is null)
            {
                return;
            }

            RefundTotalAmount -= productToRemove.UnitPrice * productToRemove.RefundQuantity;

            DebtAmount = InitialDebtAmount - RefundTotalAmount;

            if (InitialDebtAmount == RefundTotalAmount)
            {
                AmountToRefund = 0;
            }

            if (DebtAmount <= 0)
            {
                AmountToRefund = RefundTotalAmount - InitialDebtAmount;
            }

            DetailsToRefund.Remove(productToRemove);
        }

        private void OnRefundDetailChanged(object sender, EventArgs e)
        {
            RefundTotalAmount = DetailsToRefund.Sum(x => x.UnitPrice * x.RefundQuantity);

            DebtAmount = InitialDebtAmount - RefundTotalAmount;

            if (DebtAmount <= 0)
            {
                AmountToRefund = Math.Abs(RefundTotalAmount - InitialDebtAmount);
            }
            else
            {
                AmountToRefund = 0;
            }
        }
    }

    public class BindableRefundDetail : BindableBase
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int SaleQuantity { get; set; }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set => SetProperty(ref _unitPrice, value);
        }

        private decimal _totalPriceAmount;
        public decimal TotalPriceAmount
        {
            get => _totalPriceAmount;
            set => SetProperty(ref _totalPriceAmount, value);
        }

        private int _refundQuantity;
        public int RefundQuantity
        {
            get => _refundQuantity;
            set
            {
                if (value > SaleQuantity)
                {
                    return;
                }

                SetProperty(ref _refundQuantity, value);
                TotalPriceAmount = RefundQuantity * UnitPrice;
            }
        }
    }
}