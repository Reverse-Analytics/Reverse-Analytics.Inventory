using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Inventory.Modules.Customers.ViewModels.Forms;
using Inventory.Modules.Customers.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;

namespace Inventory.Modules.Customers.ViewModels
{
    public class CustomersViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly ICustomerService _customerService;

        private string _selectedCompany;
        public string SelectedCompany 
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly List<CustomerDto> customers;
        public ObservableCollection<CustomerDto> FilteredCustomers { get; private set; }
        public ObservableCollection<string> Companies { get; private set; }

        public bool KeepAlive => true;

        public CustomersViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

            AddCommand = new DelegateCommand(OnAddCustomer);
            UpdateCommand = new DelegateCommand<CustomerDto>(OnUpdateCustomer);
            ArchiveCommand = new DelegateCommand(OnArchiveCustomer);
            DeleteCommand = new DelegateCommand<CustomerDto>(OnDeleteCustomer);

            customers = new List<CustomerDto>();
            FilteredCustomers = new ObservableCollection<CustomerDto>();
            Companies = new ObservableCollection<string>();

            LoadCustomers();
        }

        private async void LoadCustomers()
        {
            var result = await _customerService.GetCustomersAsync();

            if(result == null || !result.Any())
            {
                return;
            }

            var companies = result.Select(x => x.CompanyName)
                .Distinct()
                .ToList();

            customers.AddRange(result);
            FilteredCustomers.AddRange(result);
            Companies.AddRange(companies);
        }

        private async void OnAddCustomer()
        {
            var view = new CustomerForm()
            {
                DataContext = new CustomerFormViewModel()
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result is null)
            {
                return;
            }
        }

        private async void OnUpdateCustomer(CustomerDto selectedCustomer)
        {
            var view = new CustomerForm()
            {
                DataContext = new CustomerFormViewModel(selectedCustomer)
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (result is null)
            {
                return;
            }
        }

        private void OnArchiveCustomer()
        {

        }

        private void OnDeleteCustomer(CustomerDto selectedCustomer)
        {

        }
    }
}
