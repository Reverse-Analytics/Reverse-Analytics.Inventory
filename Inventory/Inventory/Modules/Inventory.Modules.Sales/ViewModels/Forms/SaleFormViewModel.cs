using Inventory.Core.Mvvm;
using Inventory.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using ReverseAnalytics.Domain.DTOs.Customer;
using ReverseAnalytics.Domain.DTOs.Product;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inventory.Modules.Sales.ViewModels.Forms
{
    public class SaleFormViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;

        private decimal _totalDue;
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
                if (value > _totalDue) return;

                SetProperty(ref _paymentAmount, value);
                DebtAmount = TotalDue - value;
            }
        }

        private decimal _debtAmount;
        public decimal DebtAmount
        {
            get => _debtAmount;
            set => SetProperty(ref _debtAmount, value);
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private bool _canMoveToProducts = false;
        public bool CanMoveToProducts
        {
            get => _canMoveToProducts;
            set => SetProperty(ref _canMoveToProducts, value);
        }

        public ProductDto SelectedProduct { get; set; }

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

        public ObservableCollection<ProductDto> Products { get; private set; }
        public ObservableCollection<CustomerDto> Customers { get; private set; }
        public ObservableCollection<SaleDetail> AddedProducts { get; private set; }

        public DelegateCommand AddProductCommand { get; private set; }
        public DelegateCommand<SaleDetail> RemoveProductCommand { get; private set; }

        public SaleFormViewModel(ICustomerService customerService, IProductService productService, IDialogService dialogService)
        {
            TotalDue = 0;

            _customerService = customerService;
            _productService = productService;
            _dialogService = dialogService;

            Products = new ObservableCollection<ProductDto>();
            Customers = new ObservableCollection<CustomerDto>();
            AddedProducts = new ObservableCollection<SaleDetail>();
            AddedProducts.CollectionChanged += OnSaleDetailChanged;

            AddProductCommand = new DelegateCommand(OnAddProduct);
            RemoveProductCommand = new DelegateCommand<SaleDetail>(OnRemoveProduct);

            Load();
        }

        private async void Load()
        {
            try
            {
                IsBusy = true;


                var customers = await _customerService.GetCustomersAsync();
                var products = await _productService.GetProductsAsync();

                Customers.AddRange(customers);
                Products.AddRange(products);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnSaleDetailChanged(object sender, EventArgs e)
        {
            TotalDue = AddedProducts.Select(x => x.TotalPrice).Sum();
        }

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


            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }
        }

        public async void OnRemoveProduct(SaleDetail detail)
        {
            if (detail is null) return;

            AddedProducts.Remove(detail);
        }
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

        private ProductDto _product;
        public ProductDto Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;

            if (Discount > 0 && TotalPrice > 0)
            {
                var discount = (TotalPrice * (decimal)Discount) / 100;
                TotalPrice -= discount;
            }
        }
    }
}
