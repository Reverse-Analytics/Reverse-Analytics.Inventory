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
using System.Windows.Input;

namespace Inventory.Modules.Sales.ViewModels
{
    public class SalesViewModel : ViewModelBase, IRegionMemberLifetime
    {

        #region Services

        private readonly ISaleService _service;
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

        public ICommand DetailsCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand DeleteCommand { get; }

        #endregion

        #region Collections

        private readonly List<SaleDto> _sales;
        public ObservableCollection<SaleDto> Sales { get; private set; }
        public ObservableCollection<CustomerDto> Customers { get; private set; }

        #endregion

        public SalesViewModel(ISaleService service, IProductService productService, ICustomerService customerService, IDialogService dialogService)
        {
            SelectedDate = DateTime.Now;

            _service = service;
            _productService = productService;
            _customerService = customerService;
            _dialogService = dialogService;

            AddCommand = new DelegateCommand(OnAddSale);

            Sales = new ObservableCollection<SaleDto>();
            _sales = new List<SaleDto>();
            Customers = new ObservableCollection<CustomerDto>();

            LoadSales().ConfigureAwait(false);
        }

        private async Task LoadSales()
        {
            try
            {
                IsBusy = true;

                var sales = await _service.GetAllSales();
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

        private async void OnAddSale()
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

                if (result is null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message);
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
    }
}
