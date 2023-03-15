using Inventory.Core;
using Inventory.Core.Mvvm;
using Inventory.Modules.Sales.ViewModels.Forms;
using Inventory.Modules.Sales.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Customer;
using ReverseAnalytics.Domain.DTOs.Product;
using ReverseAnalytics.Domain.DTOs.Sale;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Modules.Sales.ViewModels
{
    public class SalesViewModel : ViewModelBase, IRegionMemberLifetime
    {

        #region Services

        private readonly ISaleService _saleService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Properties

        public bool KeepAlive => false;

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                UpdateDate();
            }
        }

        private CustomerDto _customer;
        public CustomerDto SelectedCustomer
        {
            get => _customer;
            set
            {
                SetProperty(ref _customer, value);
                FilterSalesByCustomer();
            }
        }

        #endregion

        #region Commands

        public DelegateCommand<SaleDto> DetailsCommand { get; }
        public DelegateCommand<SaleDto> AddCommand { get; }
        public DelegateCommand<SaleDto> UpdateCommand { get; }
        public DelegateCommand<SaleDto> DeleteCommand { get; }
        public DelegateCommand<SaleDto> PrintReceiptCommand { get; }
        public DelegateCommand<SaleDto> MakeRefundCommand { get; }

        #endregion

        #region Collections

        private readonly List<SaleDto> _sales;
        private readonly List<ProductDto> _products;
        private readonly List<CustomerDto> _customers;
        public ObservableCollection<SaleDto> Sales { get; private set; }
        public ObservableCollection<CustomerDto> Customers { get; private set; }

        #endregion

        public SalesViewModel(ISaleService service, IProductService productService, ICustomerService customerService, IDialogService dialogService)
        {
            SelectedDate = DateTime.Now;

            _saleService = service;
            _productService = productService;
            _customerService = customerService;
            _dialogService = dialogService;

            DetailsCommand = new DelegateCommand<SaleDto>(OnShowDetails);
            AddCommand = new DelegateCommand<SaleDto>(OnAddSale);
            UpdateCommand = new DelegateCommand<SaleDto>(OnUpdateSale);
            DeleteCommand = new DelegateCommand<SaleDto>(OnDeleteSale);
            PrintReceiptCommand = new DelegateCommand<SaleDto>(OnPrintReceipt);
            MakeRefundCommand = new DelegateCommand<SaleDto>(OnMakeRefund);

            _sales = new List<SaleDto>();
            _customers = new List<CustomerDto>();
            _products = new List<ProductDto>();
            Sales = new ObservableCollection<SaleDto>();
            Customers = new ObservableCollection<CustomerDto>();

            LoadSales().ConfigureAwait(false);
        }

        #region Command methods

        private async void OnShowDetails(SaleDto selectedSale)
        {
            try
            {
                var view = new SaleDetailsForm()
                {
                    DataContext = new SaleDetailsFormViewModel(selectedSale)
                };

                await DialogHost.Show(view, RegionNames.DialogRegion);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error while showing details", ex.Message);
            }
        }

        private async void OnAddSale(SaleDto selectedSale)
        {
            try
            {
                var view = new SaleForm()
                {
                    DataContext = new SaleFormViewModel(_customers, _products, _dialogService)
                };

                var result = await DialogHost.Show(view, RegionNames.DialogRegion);

                if (result is not SaleForCreateDto saleToCreate)
                {
                    return;
                }

                await _saleService.CreateSale(saleToCreate);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }
        }

        public async void OnUpdateSale(SaleDto selectedSale)
        {
            try
            {
                var view = new SaleForm()
                {
                    DataContext = new SaleFormViewModel(_customers, _products, _dialogService, selectedSale)
                };

                var result = await DialogHost.Show(view, RegionNames.DialogRegion);

                if (result is not SaleForUpdateDto saleToUpdate)
                {
                    return;
                }

                await _saleService.UpdateSale(saleToUpdate);

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error opening dialog.", ex.Message);
            }
        }

        public async void OnDeleteSale(SaleDto selectedSale)
        {

        }

        public async void OnPrintReceipt(SaleDto selectedSale)
        {

        }

        public async void OnMakeRefund(SaleDto selectedSale)
        {

        }

        #endregion

        #region Helper methods

        private async Task LoadSales()
        {
            try
            {
                IsBusy = true;

                var retreiveSales = Task.Run(async () =>
                {
                    var sales = await _saleService.GetAllSales();

                    var salesForCurrentDate = sales.Where(x => x.SaleDate.Date == DateTime.Now.Date).ToList();

                    _sales.AddRange(sales);

                    Sales.AddRange(salesForCurrentDate);
                    Customers.AddRange(_customers);
                });

                var retreiveProducts = Task.Run(async () =>
                {
                    var products = await _productService.GetProductsAsync();
                    _products.AddRange(products);
                });

                var retreiveCustomers = Task.Run(async () =>
                {
                    var customers = await _customerService.GetCustomersAsync();
                    _customers.AddRange(customers);
                });

                await Task.WhenAll(retreiveCustomers, retreiveProducts, retreiveCustomers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateDate()
        {
            if (_sales is null) return;

            if (SelectedDate is null)
            {
                Sales.Clear();
                Sales.AddRange(_sales);

                return;
            }

            var sales = _sales.Where(x => x.SaleDate.Date == SelectedDate.Value.Date).ToList();

            Sales.Clear();
            Sales.AddRange(sales);
        }

        private void FilterSalesByCustomer()
        {
            if (SelectedCustomer is null)
            {
                Sales.Clear();
                Sales.AddRange(_sales);

                return;
            }

            var sales = _sales.Where(x => x.Customer.FullName == SelectedCustomer.FullName)
                .ToList();

            Sales.AddRange(sales);
        }

        #endregion
    }
}
