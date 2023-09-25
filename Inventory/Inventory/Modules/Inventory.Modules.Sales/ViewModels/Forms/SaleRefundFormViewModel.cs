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

        private List<Core.Models.SaleDetail> saleDetails;
        public ObservableCollection<Product> RefundableProducts { get; }
        public ObservableCollection<BindableRefundDetail> DetailsToRefund { get; }

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand<BindableRefundDetail> RemoveProductCommand { get; }
        public DelegateCommand CancelCommand { get; }

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

        public SaleRefundFormViewModel(IDialogService dialogService)
        {
            saleDetails = new List<Core.Models.SaleDetail>();
            RefundableProducts = new ObservableCollection<Product>();
            DetailsToRefund = new ObservableCollection<BindableRefundDetail>();

            _dialogService = dialogService;

            CancelCommand = new DelegateCommand(OnCancel);
            AddProductCommand = new DelegateCommand(OnAddProduct);
            RemoveProductCommand = new DelegateCommand<BindableRefundDetail>(OnDeleteProduct);
        }

        public SaleRefundFormViewModel(Sale sale, IDialogService dialogService)
            : this(dialogService)
        {
            SetupCollections(sale);
            SetupSaleDebt(sale);
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

            var saleDetailToRefund = saleDetails.FirstOrDefault(x => x.ProductId == SelectedProduct.Id);

            if (saleDetailToRefund is null || DetailsToRefund.Any(x => x.ProductId == saleDetailToRefund.ProductId))
            {
                return;
            }

            var saleDetailToAdd = new BindableRefundDetail
            {
                UnitPrice = saleDetailToRefund.UnitPrice,
                TotalDiscount = saleDetailToRefund.Discount,
                TotalDue = saleDetailToRefund.TotalDue,
                ProductCode = SelectedProduct.ProductCode,
                SaleQuantity = saleDetailToRefund.Quantity,
                ProductId = SelectedProduct.Id,
                PriceWithDiscount = saleDetailToRefund.TotalDue / saleDetailToRefund.Quantity,
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

            RefundTotalAmount -= productToRemove.PriceWithDiscount * productToRemove.RefundQuantity;

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
            RefundTotalAmount = DetailsToRefund.Sum(x => x.PriceWithDiscount * x.RefundQuantity);

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
        public decimal UnitPrice { get; set; }
        public string ProductCode { get; set; }
        public decimal TotalDue { get; set; }
        public double DiscountPercentage { get; set; }
        public decimal TotalDiscount { get; set; }
        public int SaleQuantity { get; set; }
        public decimal PriceWithDiscount { get; set; }
        public decimal TotalPriceAmount { get; set; }

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
                CalculateTotals();
            }
        }

        private void CalculateTotals()
        {
            // calculate discount percentage based on TotalDiscount and TotalDue
            TotalDue = RefundQuantity * PriceWithDiscount;
            TotalPriceAmount = RefundQuantity * PriceWithDiscount;
        }
    }
}