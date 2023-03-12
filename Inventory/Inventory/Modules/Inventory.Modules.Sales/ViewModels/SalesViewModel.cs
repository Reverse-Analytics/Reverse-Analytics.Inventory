using Inventory.Core;
using Inventory.Core.Mvvm;
using Inventory.Modules.Sales.ViewModels.Forms;
using Inventory.Modules.Sales.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.Customer;
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

            Sales = new ObservableCollection<SaleDto>();
            _sales = new List<SaleDto>();
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
                var customers = await _customerService.GetCustomersAsync();
                var products = await _productService.GetProductsAsync();

                var view = new SaleForm()
                {
                    DataContext = new SaleFormViewModel(customers.ToList(), products.ToList(), _dialogService)
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

                var sales = await _saleService.GetAllSales();
                var currentDateSales = sales.Where(x => x.SaleDate.Date == DateTime.Now.Date).ToList();

                _sales.AddRange(sales);
                Sales.AddRange(currentDateSales);
                var customers = sales.Select(x => x.Customer).Distinct().ToList();
                Customers.AddRange(customers);
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
