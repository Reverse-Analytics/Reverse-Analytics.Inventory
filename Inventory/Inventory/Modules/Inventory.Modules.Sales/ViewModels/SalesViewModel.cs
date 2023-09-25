using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Modules.Sales.ViewModels.Forms;
using Inventory.Modules.Sales.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
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

        private Customer _customer;
        public Customer SelectedCustomer
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

        public DelegateCommand<Sale> DetailsCommand { get; }
        public DelegateCommand<Sale> AddCommand { get; }
        public DelegateCommand<Sale> UpdateCommand { get; }
        public DelegateCommand<Sale> DeleteCommand { get; }
        public DelegateCommand<Sale> PrintReceiptCommand { get; }
        public DelegateCommand<Sale> MakeRefundCommand { get; }

        #endregion

        #region Collections

        private readonly List<Sale> _sales;
        private readonly List<Product> _products;
        private readonly List<Customer> _customers;
        public ObservableCollection<Sale> Sales { get; private set; }
        public ObservableCollection<Customer> Customers { get; private set; }

        #endregion

        public SalesViewModel(ISaleService service, IProductService productService, ICustomerService customerService, IDialogService dialogService)
        {
            SelectedDate = DateTime.Now;

            _saleService = service;
            _productService = productService;
            _customerService = customerService;
            _dialogService = dialogService;

            DetailsCommand = new DelegateCommand<Sale>(OnShowDetails);
            AddCommand = new DelegateCommand<Sale>(OnAddSale);
            UpdateCommand = new DelegateCommand<Sale>(OnUpdateSale);
            DeleteCommand = new DelegateCommand<Sale>(OnDeleteSale);
            PrintReceiptCommand = new DelegateCommand<Sale>(OnPrintReceipt);
            MakeRefundCommand = new DelegateCommand<Sale>(OnMakeRefund);

            _sales = new List<Sale>();
            _customers = new List<Customer>();
            _products = new List<Product>();
            Sales = new ObservableCollection<Sale>();
            Customers = new ObservableCollection<Customer>();

            LoadSales().ConfigureAwait(false);
        }

        #region Command methods

        private async void OnShowDetails(Sale selectedSale)
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

        private async void OnAddSale(Sale selectedSale)
        {
            try
            {
                var view = new SaleForm()
                {
                    DataContext = new SaleFormViewModel(_customers, _products, _dialogService)
                };

                var result = await DialogHost.Show(view, RegionNames.DialogRegion);

                if (result is not Sale saleToCreate)
                {
                    return;
                }

                var createdSale = await _saleService.CreateSale(saleToCreate);

                Sales.Insert(0, createdSale);
                _sales.Add(createdSale);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
            }
        }

        public async void OnUpdateSale(Sale selectedSale)
        {
            try
            {
                var view = new SaleForm()
                {
                    DataContext = new SaleFormViewModel(_customers, _products, _dialogService, selectedSale)
                };

                var result = await DialogHost.Show(view, RegionNames.DialogRegion);

                if (result is not Sale saleToUpdate)
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

        public async void OnDeleteSale(Sale selectedSale)
        {
            try
            {
                var result = await _dialogService.ShowConfirmation("Confirm your action", "Are you sure you want to delete sale?");

                if (result is not true)
                {
                    return;
                }

                await _saleService.DeleteSale(selectedSale.Id);

                Sales.Remove(selectedSale);
                _sales.Remove(selectedSale);

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error occured while executing the operation.", ex.Message);
            }
        }

        public async void OnPrintReceipt(Sale selectedSale)
        {

        }

        public async void OnMakeRefund(Sale selectedSale)
        {
            try
            {
                var view = new SaleRefundForm()
                {
                    DataContext = new SaleRefundFormViewModel(selectedSale, _dialogService)
                };

                var result = await DialogHost.Show(view, RegionNames.DialogRegion);

                if (result is not Sale saleToUpdate)
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

                await Task.WhenAll(retreiveCustomers, retreiveProducts, retreiveSales);
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
