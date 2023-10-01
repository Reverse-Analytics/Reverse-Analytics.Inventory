using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Modules.Sales.Models;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Inventory.Modules.Sales.ViewModels.Forms
{
    internal class SaleRefundFormViewModel : ViewModelBase
    {

        #region Services

        private readonly IDialogService _dialogService;

        #endregion

        #region Collections

        private readonly List<SaleDetail> currentSaleDetails;
        public List<Sale> Sales { get; private set; }
        public ObservableCollection<Product> RefundableProducts { get; }
        public ObservableCollection<BindableRefundDetail> DetailsToRefund { get; }

        #endregion

        #region Commands

        public DelegateCommand AddProductCommand { get; }
        public DelegateCommand RefundAllCommand { get; }
        public DelegateCommand<BindableRefundDetail> RemoveProductCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }

        #endregion

        #region Properties

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
                SaleUpdated();
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private Refund _refund;
        public Refund Refund
        {
            get => _refund;
            set => SetProperty(ref _refund, value);
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
            set
            {
                SetProperty(ref _refundTotalAmount, value);

                CalculateTotals();
            }
        }

        private decimal _amountToRefund;
        public decimal AmountToRefund
        {
            get => _amountToRefund;
            set
            {
                SetProperty(ref _amountToRefund, value);
            }
        }

        private decimal _debtPaymentAmount;
        public decimal DebtPaymentAmount
        {
            get => _debtPaymentAmount;
            set
            {
                SetProperty(ref _debtPaymentAmount, value);
            }
        }

        private decimal _debtAmount;
        public decimal DebtAmount
        {
            get => _debtAmount;
            set
            {
                SetProperty(ref _debtAmount, value);
                DebtText = $"Debt ({value:N2}): ";
            }
        }

        // Inidicates whether another sale can be chosen
        // to refund, if refund is being created from sale
        // page, then it shouldn't be allowed to change
        private bool _isSaleSelectionEnabled;
        public bool IsSaleSelectionEnabled
        {
            get => _isSaleSelectionEnabled;
            set => SetProperty(ref _isSaleSelectionEnabled, value);
        }

        private string _debtText = $"Debt (0.00): ";
        public string DebtText
        {
            get => _debtText;
            set => SetProperty(ref _debtText, value);
        }

        private Visibility _debtPaymentVisibility = Visibility.Collapsed;
        public Visibility DebtPaymentVisibility
        {
            get => _debtPaymentVisibility;
            set => SetProperty(ref _debtPaymentVisibility, value);
        }

        private bool _subtractFromDebt;
        public bool SubtractFromDebt
        {
            get => _subtractFromDebt;
            set
            {
                SetProperty(ref _subtractFromDebt, value);
                CalculateTotals();
            }
        }

        #endregion

        #region Constructors

        public SaleRefundFormViewModel(IDialogService dialogService, List<Sale> sales)
        {
            _dialogService = dialogService;

            currentSaleDetails = new List<SaleDetail>();
            RefundableProducts = new ObservableCollection<Product>();
            DetailsToRefund = new ObservableCollection<BindableRefundDetail>();
            Sales = sales ?? new List<Sale>();

            AddProductCommand = new DelegateCommand(OnAddProduct);
            RefundAllCommand = new DelegateCommand(OnRefundAll);
            RemoveProductCommand = new DelegateCommand<BindableRefundDetail>(OnRemoveProduct);
            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(OnCancel);

            IsSaleSelectionEnabled = true;
        }

        public SaleRefundFormViewModel(IDialogService dialogService, List<Sale> sales, Sale sale)
            : this(dialogService, sales)
        {
            SelectedSale = sale;
            IsSaleSelectionEnabled = false;
        }

        public SaleRefundFormViewModel(IDialogService dialogService, List<Sale> sales, Sale sale, Refund refund)
            : this(dialogService, sales, sale)
        {
            Refund = refund;
            SetupInitialValues(refund);
        }

        #endregion

        #region Command methods

        public void OnAddProduct()
        {
            if (!TryGetSaleDetail(out SaleDetail saleDetailToRefund))
            {
                return;
            }

            int refundId = 0;
            if (Refund is not null)
            {
                refundId = Refund.RefundDetails.FirstOrDefault(x => x.ProductId == saleDetailToRefund.ProductId)?.Id ?? 0;
            }

            var saleDetailToAdd = new BindableRefundDetail
            {
                Id = refundId,
                ProductId = saleDetailToRefund.ProductId,
                ProductCode = SelectedProduct.ProductCode,
                UnitPrice = saleDetailToRefund.UnitPrice,
                SaleQuantity = saleDetailToRefund.Quantity,
            };

            saleDetailToAdd.PropertyChanged += OnRefundDetailChanged;

            DetailsToRefund.Add(saleDetailToAdd);
        }

        private void OnRefundAll()
        {
            if (!currentSaleDetails.Any())
            {
                return;
            }

            List<BindableRefundDetail> detailsToRefund = new List<BindableRefundDetail>();
            RefundTotalAmount = 0;

            foreach (var detail in currentSaleDetails)
            {
                var detailToRefund = new BindableRefundDetail
                {
                    ProductId = detail.ProductId,
                    ProductCode = detail.Product.ProductCode,
                    UnitPrice = detail.UnitPrice,
                    SaleQuantity = detail.Quantity,
                    RefundQuantity = detail.Quantity,
                };
                detailToRefund.PropertyChanged += OnRefundDetailChanged;
                detailsToRefund.Add(detailToRefund);

                RefundTotalAmount += detailToRefund.UnitPrice * detailToRefund.RefundQuantity;
            }

            DetailsToRefund.Clear();
            DetailsToRefund.AddRange(detailsToRefund);
        }

        public void OnRemoveProduct(BindableRefundDetail product)
        {
            var productToRemove = DetailsToRefund.FirstOrDefault(x => x.ProductId == product.ProductId);

            if (productToRemove is null)
            {
                return;
            }

            RefundTotalAmount -= productToRemove.UnitPrice * productToRemove.RefundQuantity;

            DetailsToRefund.Remove(productToRemove);
        }

        public async void OnCancel()
        {
            var result = await _dialogService.ShowConfirmation("Please, confirm the action.", "Are you sure you want to close the dialog?");

            if (result)
            {
                DialogHost.Close(RegionNames.DialogRegion);
            }
        }

        private async void OnSave()
        {
            if (!CanSave())
            {
                return;
            }

            var result = await _dialogService.ShowConfirmation("Please, confirm the action.", "Are you sure you want to save the refund?");

            if (result is true)
            {
                var refund = GenerateRefund();

                DialogHost.Close(RegionNames.DialogRegion, refund);
            }
        }

        #endregion

        #region Helper methods

        private void OnRefundDetailChanged(object sender, EventArgs e)
        {
            RefundTotalAmount = DetailsToRefund.Sum(x => x.UnitPrice * x.RefundQuantity);
        }

        private void SaleUpdated()
        {
            if (SelectedSale is null)
            {
                return;
            }

            if (SelectedSale.SaleDebt != null)
            {
                DebtAmount = SelectedSale.SaleDebt.TotalDue - SelectedSale.SaleDebt.TotalPaid;
                DebtPaymentVisibility = Visibility.Visible;
            }
            else
            {
                DebtPaymentVisibility = Visibility.Collapsed;
            }

            var refundableProducts = SelectedSale.SaleDetails
                .Select(sd => sd.Product)
                .Distinct()
                .ToList();

            RefundableProducts.Clear();
            RefundableProducts.AddRange(refundableProducts);

            currentSaleDetails.Clear();
            currentSaleDetails.AddRange(SelectedSale.SaleDetails);

            DetailsToRefund.Clear();

            RefundTotalAmount = 0;
        }

        private void SetupInitialValues(Refund refund)
        {
            foreach (var detail in refund.RefundDetails)
            {
                if (detail is null)
                {
                    continue;
                }

                var saleDetail = refund.Sale.SaleDetails.FirstOrDefault(x => x.Product.Id == detail.Product.Id);

                if (saleDetail is null)
                {
                    return;
                }

                var saleDetailToAdd = new BindableRefundDetail()
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    ProductCode = detail.Product.ProductCode,
                    SaleQuantity = saleDetail.Quantity,
                    UnitPrice = saleDetail.UnitPrice,
                    RefundQuantity = detail.Quantity,
                };
                saleDetailToAdd.PropertyChanged += OnRefundDetailChanged;

                DetailsToRefund.Add(saleDetailToAdd);
            }

            RefundTotalAmount = DetailsToRefund.Sum(x => x.UnitPrice * x.RefundQuantity);
            DebtPaymentAmount = refund.DebtPaymentAmount;
            AmountToRefund = refund.RefundAmount;
            DebtAmount += refund.DebtPaymentAmount;
        }

        private bool TryGetSaleDetail(out SaleDetail saleDetail)
        {
            if (SelectedProduct == null)
            {
                saleDetail = null;
                return false;
            }

            var saleDetailToRefund = currentSaleDetails.FirstOrDefault(x => x.Product.Id == SelectedProduct.Id);

            if (saleDetailToRefund == null)
            {
                saleDetail = null;
                return false;
            }

            if (DetailsToRefund.Any(x => x.ProductId == saleDetailToRefund.ProductId))
            {
                saleDetail = null;
                return false;
            }

            saleDetail = saleDetailToRefund;
            return true;
        }

        private bool CanSave()
        {
            if (SelectedSale is null)
            {
                return false;
            }

            if (!DetailsToRefund.Any())
            {
                return false;
            }

            return true;
        }

        private void CalculateTotals()
        {
            if (!SubtractFromDebt)
            {
                AmountToRefund = RefundTotalAmount;
                DebtPaymentAmount = 0;

                return;
            }

            if (RefundTotalAmount > DebtAmount)
            {
                AmountToRefund = RefundTotalAmount - DebtAmount;
                DebtPaymentAmount = DebtAmount;
            }
            else
            {
                AmountToRefund = 0;
                DebtPaymentAmount = RefundTotalAmount;
            }
        }

        private Refund GenerateRefund() => new()
        {
            Id = Refund?.Id ?? 0,
            SaleId = SelectedSale.Id,
            RefundDate = SelectedDate,
            TotalAmount = RefundTotalAmount,
            DebtPaymentAmount = DebtPaymentAmount,
            RefundAmount = AmountToRefund,
            RefundDetails = DetailsToRefund.Select(x => new RefundDetail
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Quantity = x.RefundQuantity,
                RefundId = Refund?.Id ?? 0,
            }).ToList(),
        };

        #endregion
    }
}