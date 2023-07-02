using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using Inventory.Modules.Customers.ViewModels.Forms;
using Inventory.Modules.Customers.Views.Forms;
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
using System.Windows.Input;

namespace Inventory.Modules.Customers.ViewModels
{
    public class CustomersViewModel : ViewModelBase, IRegionMemberLifetime
    {

        #region Services

        private readonly ICustomerService _customerService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Properties

        private string _selectedCompany;
        public string SelectedCompany
        {
            get => _selectedCompany;
            set => SetProperty(ref _selectedCompany, value);
        }

        public bool KeepAlive => false;

        #endregion

        #region Commands

        public ICommand DetailsCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand DeleteCommand { get; }

        #endregion

        #region Collections

        private readonly List<Customer> customers;
        public ObservableCollection<Customer> FilteredCustomers { get; private set; }
        public ObservableCollection<string> Companies { get; private set; }

        #endregion

        public CustomersViewModel(ICustomerService customerService, IDialogService dialogService)
        {
            _customerService = customerService;
            _dialogService = dialogService;

            DetailsCommand = new DelegateCommand<CustomerDto>(OnShowDetails);
            AddCommand = new DelegateCommand(OnAddCustomer);
            UpdateCommand = new DelegateCommand<CustomerDto>(OnUpdateCustomer);
            ArchiveCommand = new DelegateCommand(OnArchiveCustomer);
            DeleteCommand = new DelegateCommand<CustomerDto>(OnDeleteCustomer);

            customers = new List<CustomerDto>();
            FilteredCustomers = new ObservableCollection<CustomerDto>();
            Companies = new ObservableCollection<string>();

            LoadCustomers();
        }

        #region Main methods

        private async void LoadCustomers()
        {
            try
            {
                IsBusy = true;

                var result = await _customerService.GetCustomersAsync();

                if (result == null) return;

                var companies = result.Select(x => x.CompanyName)
                    .Distinct()
                    .ToList();

                customers.AddRange(result);
                FilteredCustomers.AddRange(result);
                Companies.AddRange(companies);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Command methods

        private async void OnAddCustomer()
        {
            try
            {
                var result = await ShowAddCustomerForm();

                if (result is null) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    var createdCustomer = await _customerService.CreateCustomerAsync(result);

                    FilteredCustomers.Insert(0, createdCustomer);
                    customers.Add(createdCustomer);
                });

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnUpdateCustomer(Customer selectedCustomer)
        {
            try
            {
                var customer = FilteredCustomers.FirstOrDefault(x => x.Id == selectedCustomer.Id);
                var result = await ShowUpdateCustomerForm(customer);

                if (result is null) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _customerService.UpdateCustomerAsync(result);
                });

                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnDeleteCustomer(Customer selectedCustomer)
        {
            try
            {
                bool isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;

                IsBusy = true;

                await Task.Run(async () =>
                {
                    await _customerService.DeleteCustomerAsync(selectedCustomer.Id);

                    FilteredCustomers.Remove(selectedCustomer);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnArchiveCustomer()
        {
            try
            {
                bool isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;

                IsBusy = true;

                // TODO implement archive call
                await Task.Delay(1500);
                await _dialogService.ShowSuccess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await _dialogService.ShowError();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnShowDetails(Customer customerDto)
        {
            var view = new CustomerDetailsForm()
            {
                DataContext = new CustomerDetailsFormViewModel(customerDto)
            };

            await DialogHost.Show(view, "RootDialog");
        }

        #endregion

        #region Helper methods

        public async Task<Customer> ShowAddCustomerForm()
        {
            var view = new CustomerForm()
            {
                DataContext = new CustomerFormViewModel()
            };

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            return result as Customer;
        }

        private async Task<Customer> ShowUpdateCustomerForm(Customer customer)
        {
            var view = new CustomerForm()
            {
                DataContext = new CustomerFormViewModel(customer)
            };

            var result = await DialogHost.Show(view, RegionNames.DialogRegion);

            return result as CustomerForUpdateDto;
        }

        #endregion
    }
}
