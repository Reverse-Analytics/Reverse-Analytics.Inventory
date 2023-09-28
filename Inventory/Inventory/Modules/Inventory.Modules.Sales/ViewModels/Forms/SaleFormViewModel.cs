using Inventory.Core;
using Inventory.Core.Enums;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Modules.Sales.Models;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inventory.Modules.Sales.ViewModels.Forms
{
    public class SaleFormViewModel : ViewModelBase
    {
        #region Services

        private readonly IDialogService _dialogService;

        #endregion

        #region Sale Details

        public int SaleId { get; set; }

        private decimal _totalDue = 0;
        public decimal TotalDue
        {
            get => _totalDue;
            set
            {
                SetProperty(ref _totalDue, value);
                UpdateDebtAmount();
            }
        }

        private decimal _paymentAmount;
        public decimal PaymentAmount
        {
            get => _paymentAmount;
            set
            {
                if (value > _totalDue) value = _totalDue;

                SetProperty(ref _paymentAmount, value);
                UpdateDebtAmount();
            }
        }

        private decimal _discountTotal;
        public decimal DiscountTotal
        {
            get => _discountTotal;
            set => SetProperty(ref _discountTotal, value);
        }

        private decimal _debtAmount;
        public decimal DebtAmount
        {
            get => _debtAmount;
            set => SetProperty(ref _debtAmount, value);
        }

        private string _comments;
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        private CurrencyType _currencyType;
        public CurrencyType CurrencyType
        {
            get => _currencyType;
            set => SetProperty(ref _currencyType, value);
        }

        private PaymentType _paymentType;
        public PaymentType PaymentType
        {
            get => _paymentType;
            set => SetProperty(ref _paymentType, value);
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                CanMoveToProducts = value != null;
            }
        }

        public static List<CurrencyType> CurrencyTypes => new() { CurrencyType.UZS, CurrencyType.USD };
        public static List<PaymentType> PaymentTypes => new() { PaymentType.Cash, PaymentType.Card };

        #endregion

        #region UI Properties

        private bool _canMoveToProducts = false;
        public bool CanMoveToProducts
        {
            get => _canMoveToProducts;
            set => SetProperty(ref _canMoveToProducts, value);
        }

        private bool _canMoveToPayment = false;
        public bool CanMoveToPayment
        {
            get => _canMoveToPayment;
            set => SetProperty(ref _canMoveToPayment, value);
        }

        private bool _isDatePickerEnabled = true;
        public bool IsDatePickerEnabled
        {
            get => _isDatePickerEnabled;
            set => SetProperty(ref _isDatePickerEnabled, value);
        }

        #endregion

        #region Commands

        public DelegateCommand AddProductCommand { get; private set; }
        public DelegateCommand<BindableSaleDetail> RemoveProductCommand { get; private set; }
        public DelegateCommand MakePaymentCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        #endregion

        #region Collections

        public List<Product> Products { get; private set; }
        public List<Customer> Customers { get; private set; }
        public ObservableCollection<BindableSaleDetail> AddedProducts { get; private set; }

        #endregion

        #region Constructors

        public SaleFormViewModel(List<Customer> customers, List<Product> products, IDialogService dialogService)
        {
            _dialogService = dialogService;

            Products = products;
            Customers = customers;
            AddedProducts = new ObservableCollection<BindableSaleDetail>();
            AddedProducts.CollectionChanged += OnSaleDetailChanged;

            AddProductCommand = new DelegateCommand(OnAddProduct);
            RemoveProductCommand = new DelegateCommand<BindableSaleDetail>(OnRemoveProduct);
            MakePaymentCommand = new DelegateCommand(OnMakePayment);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        public SaleFormViewModel(List<Customer> customers, List<Product> products, IDialogService dialogService, Sale sale)
            : this(customers, products, dialogService)
        {
            Customers = customers;

            SelectedCustomer = Customers.FirstOrDefault(x => x.Id == sale.CustomerId);
            SelectedDate = sale.SaleDate;
            Comments = sale.Comments;
            TotalDue = sale.TotalDue;
            PaymentAmount = sale.TotalPaid;
            SaleId = sale.Id;

            IsDatePickerEnabled = false;
            CanMoveToPayment = true;

            SetupDetails(sale);
        }

        #endregion

        #region Command methods

        public async void OnAddProduct()
        {
            try
            {
                if (SelectedProduct is null)
                {
                    return;
                }

                var productAlreadyAdded = AddedProducts.Any(x => x.ProductId == SelectedProduct.Id);

                if (productAlreadyAdded)
                {
                    return;
                }

                var saleDetail = new BindableSaleDetail()
                {
                    ProductId = SelectedProduct.Id,
                    ProductCode = SelectedProduct.ProductCode,
                    SalePrice = SelectedProduct.SalePrice,
                    Quantity = 1,
                    UnitPrice = SelectedProduct.SalePrice,
                };

                saleDetail.PropertyChanged += OnSaleDetailChanged;

                AddedProducts.Add(saleDetail);

                CanMoveToPayment = true;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }
        }

        public async void OnRemoveProduct(BindableSaleDetail detail)
        {
            try
            {
                if (detail is null) return;

                AddedProducts.Remove(detail);

                CanMoveToPayment = AddedProducts.Count > 0;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error removing product.", ex.Message);
            }
        }

        public async void OnMakePayment()
        {
            try
            {
                var result = await _dialogService.ShowConfirmation();

                if (!result) return;

                var sale = new Sale
                {
                    Id = SaleId,
                    CustomerId = SelectedCustomer.Id,
                    SaleDate = SelectedDate,
                    SaleDetails = GetSaleDetails(),
                    TotalDue = TotalDue,
                    TotalPaid = PaymentAmount,
                    TotalDiscount = DiscountTotal,
                    SaleType = Core.Enums.SaleType.Retaile,
                    Comments = Comments,
                    Receipt = $"{SelectedDate.Date:dd-MM-yyyy} {Guid.NewGuid().ToString()[5..]}",
                    CurrencyType = CurrencyType,
                    PaymentType = PaymentType
                };

                DialogHost.Close(RegionNames.DialogRegion, sale);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error completing sale.", ex.Message);
            }
        }

        private async void OnCancel()
        {
            var result = await _dialogService.ShowConfirmation("Please, confirm the action.", "Are you sure you want to close the dialog?");

            if (result)
            {
                DialogHost.Close(RegionNames.DialogRegion);
            }
        }

        #endregion

        #region Helper methods

        private void OnSaleDetailChanged(object sender, EventArgs e)
        {
            TotalDue = AddedProducts.Select(x => x.TotalPrice).Sum();
            DiscountTotal = AddedProducts.Sum(x => x.TotalDiscount);

            if (PaymentAmount > TotalDue)
            {
                PaymentAmount = TotalDue;
            }
        }

        private void SetupDetails(Sale sale)
        {
            if (!sale?.SaleDetails?.Any() ?? false)
            {
                return;
            }

            sale.SaleDetails.ForEach(x =>
            {
                var saleDetail = new BindableSaleDetail()
                {
                    Id = x.Id,
                    ProductId = x.Product.Id,
                    ProductCode = x.Product.ProductCode,
                    SalePrice = x.Product.SalePrice,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                };

                saleDetail.PropertyChanged += OnSaleDetailChanged;

                AddedProducts.Add(saleDetail);
            });

            PaymentAmount = sale.TotalPaid;

            SelectedCustomer = Customers.FirstOrDefault(x => x.Id == sale.Customer.Id);
        }

        private void UpdateDebtAmount()
        {
            DebtAmount = TotalDue - PaymentAmount;
        }

        private List<SaleDetail> GetSaleDetails() =>
            AddedProducts.Select(x => new Inventory.Core.Models.SaleDetail
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Discount = x.TotalDiscount,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                SaleId = SaleId,
                TotalDue = x.TotalPrice,
            }).ToList();

        #endregion
    }
}
