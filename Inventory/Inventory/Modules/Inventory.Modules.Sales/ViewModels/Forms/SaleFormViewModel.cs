using Inventory.Core;
using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using ReverseAnalytics.Domain.DTOs.Customer;
using ReverseAnalytics.Domain.DTOs.Product;
using ReverseAnalytics.Domain.DTOs.Sale;
using ReverseAnalytics.Domain.DTOs.SaleDetail;
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

        #region Properties

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

        private decimal _totalDueWithDiscount = 0;
        public decimal TotalDueWithDiscount
        {
            get => _totalDueWithDiscount;
            set => SetProperty(ref _totalDueWithDiscount, value);
        }

        private decimal _paymentAmount;
        public decimal PaymentAmount
        {
            get => _paymentAmount;
            set
            {
                if (value > _totalDue) value = _totalDue;

                SetProperty(ref _paymentAmount, value);
                DebtAmount = TotalDueWithDiscount - value;
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

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private ProductDto _selectedProduct;
        public ProductDto SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private CustomerDto _selectedCustomer;
        public CustomerDto SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                CanMoveToProducts = value != null;
            }
        }

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

        #endregion

        #region Commands

        public DelegateCommand AddProductCommand { get; private set; }
        public DelegateCommand<SaleDetail> RemoveProductCommand { get; private set; }
        public DelegateCommand MakePaymentCommand { get; private set; }

        #endregion

        #region Collections

        public ObservableCollection<ProductDto> Products { get; private set; }
        public ObservableCollection<CustomerDto> Customers { get; private set; }
        public ObservableCollection<SaleDetail> AddedProducts { get; private set; }

        #endregion

        public SaleFormViewModel(List<CustomerDto> customers, List<ProductDto> products, IDialogService dialogService)
        {
            _dialogService = dialogService;

            Products = new ObservableCollection<ProductDto>(products);
            Customers = new ObservableCollection<CustomerDto>(customers);
            AddedProducts = new ObservableCollection<SaleDetail>();
            AddedProducts.CollectionChanged += OnSaleDetailChanged;

            AddProductCommand = new DelegateCommand(OnAddProduct);
            RemoveProductCommand = new DelegateCommand<SaleDetail>(OnRemoveProduct);
            MakePaymentCommand = new DelegateCommand(OnMakePayment);
        }

        #region Command methods

        public async void OnAddProduct()
        {
            try
            {
                if (SelectedProduct is null) return;

                var saleDetail = new SaleDetail
                {
                    Quantity = 0,
                    UnitPrice = SelectedProduct.SalePrice,
                    ProductId = SelectedProduct.Id,
                    Product = SelectedProduct
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

                var sale = new SaleForCreateDto
                {
                    CustomerId = SelectedCustomer.Id,
                    SaleDate = SelectedDate,
                    SaleDetails = GetSaleDetails(),
                    TotalDue = TotalDue,
                    TotalPaid = PaymentAmount,
                    DiscountTotal = DiscountTotal,
                    SaleType = ReverseAnalytics.Domain.Enums.SaleType.Retaile,
                    Comment = Comments,
                    Status = DebtAmount == 0 ?
                        ReverseAnalytics.Domain.Enums.TransactionStatus.Finished :
                        ReverseAnalytics.Domain.Enums.TransactionStatus.Debt,
                    Receipt = $"{SelectedDate.Date:dd-MM-yyyy} {Guid.NewGuid().ToString()[5..]}"
                };

                DialogHost.Close(RegionNames.DialogRegion, sale);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error completing sale.", ex.Message);
            }
        }

        private List<SaleDetailDto> GetSaleDetails()
        {
            return AddedProducts.Select(x => new SaleDetailDto
            {
                ProductId = x.ProductId,
                Discount = x.Discount,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList();
        }

        #endregion

        #region Helper methods

        private void OnSaleDetailChanged(object sender, EventArgs e)
        {
            TotalDue = AddedProducts.Select(x => x.TotalPrice).Sum();
            TotalDueWithDiscount = AddedProducts.Sum(x => x.TotalPriceWithDiscount);
            DiscountTotal = AddedProducts.Sum(x => x.CalculateTotalDiscount());
        }

        #endregion
    }

    public class SaleDetail : BindableBase
    {
        public int ProductId { get; set; }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                SetProperty(ref _unitPrice, value);
                CalculateTotalPrice();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                CalculateTotalPrice();
            }
        }

        private double _discount;
        public double Discount
        {
            get => _discount;
            set
            {
                if (value < 100)
                {
                    SetProperty(ref _discount, value);
                    CalculateTotalPrice();
                }
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private decimal _totalPriceWithDiscount;
        public decimal TotalPriceWithDiscount
        {
            get => _totalPriceWithDiscount;
            set => SetProperty(ref _totalPriceWithDiscount, value);
        }

        private ProductDto _product;
        public ProductDto Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;

            // Calculate discount total
            if (UnitPrice > 0)
            {
                var discount = (TotalPrice * (decimal)Discount) / 100;
                TotalPriceWithDiscount = TotalPrice - discount;
            }
        }

        public decimal CalculateTotalDiscount()
        {
            if (UnitPrice > 0 && Discount > 0)
                return (TotalPrice * (decimal)Discount) / 100;

            return 0;
        }
    }
}
