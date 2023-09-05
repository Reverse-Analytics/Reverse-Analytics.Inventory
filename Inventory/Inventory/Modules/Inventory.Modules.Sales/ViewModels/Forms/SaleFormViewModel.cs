using Inventory.Core;
using Inventory.Core.Enums;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
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
                DebtAmount = value;
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
                DebtAmount = TotalDue - value;
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
        public DelegateCommand<SaleDetail> RemoveProductCommand { get; private set; }
        public DelegateCommand MakePaymentCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        #endregion

        #region Collections

        public List<Product> Products { get; private set; }
        public List<Customer> Customers { get; private set; }
        public ObservableCollection<SaleDetail> AddedProducts { get; private set; }

        #endregion

        public SaleFormViewModel(List<Customer> customers, List<Product> products, IDialogService dialogService)
        {
            _dialogService = dialogService;

            Products = products;
            Customers = customers;
            AddedProducts = new ObservableCollection<SaleDetail>();
            AddedProducts.CollectionChanged += OnSaleDetailChanged;

            AddProductCommand = new DelegateCommand(OnAddProduct);
            RemoveProductCommand = new DelegateCommand<SaleDetail>(OnRemoveProduct);
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

                var saleDetail = new SaleDetail
                {
                    ProductCode = SelectedProduct.ProductCode,
                    Quantity = 1,
                    UnitPrice = SelectedProduct.SalePrice,
                    ProductId = SelectedProduct.Id
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

        public async void OnRemoveProduct(SaleDetail detail)
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

        private List<Inventory.Core.Models.SaleDetail> GetSaleDetails() =>
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
            DiscountTotal = AddedProducts.Sum(x => x.CalculateTotalDiscount());
        }

        private void SetupDetails(Sale sale)
        {
            if (!sale?.SaleDetails?.Any() ?? false)
            {
                return;
            }

            sale.SaleDetails.ForEach(x => AddedProducts.Add(new SaleDetail()
            {
                Id = x.Id,
                ProductCode = x.Product.ProductCode,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalDiscount = x.Discount,
                ProductId = x.ProductId,
            }));
        }

        #endregion
    }

    public class SaleDetail : BindableBase
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                SetProperty(ref _unitPrice, value);
                CalculateTotals();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                CalculateTotals();
            }
        }

        private decimal _discountPercentage;
        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (value < 100)
                {
                    SetProperty(ref _discountPercentage, value);
                    CalculateTotals();
                }
            }
        }

        private decimal _totalDiscount;
        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set
            {
                if (_totalDiscount == value) return;

                SetProperty(ref _totalDiscount, value);
                CalcualteDiscountPercentage();
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private void CalculateTotals()
        {
            TotalDiscount = CalculateTotalDiscount();
            TotalPrice = CalculateTotalPrice();
        }

        public decimal CalculateTotalDiscount()
        {
            if (UnitPrice <= 0 || Quantity <= 0)
                return 0;

            var totalPrice = UnitPrice * Quantity;

            return (totalPrice * DiscountPercentage) / 100;
        }

        private decimal CalculateTotalPrice()
        {
            if (UnitPrice <= 0 || Quantity <= 0)
                return 0;

            return (UnitPrice * Quantity) - TotalDiscount;
        }

        private void CalcualteDiscountPercentage()
        {
            if (TotalDiscount > 0 && UnitPrice > 0)
            {
                DiscountPercentage = TotalDiscount * 100 / (UnitPrice * Quantity);
            }
        }
    }

}
