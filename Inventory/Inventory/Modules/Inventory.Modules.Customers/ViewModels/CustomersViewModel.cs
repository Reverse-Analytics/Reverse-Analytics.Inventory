using Inventory.Core.Mvvm;
using Inventory.Modules.Customers.ViewModels.Forms;
using Inventory.Modules.Customers.Views.Forms;
using Inventory.Services.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Regions;
using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;
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

        public bool KeepAlive => true;

        #endregion

        #region Commands

        public ICommand DetailsCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand DeleteCommand { get; }

        #endregion

        #region Collections

        private readonly List<CustomerDto> customers;
        public ObservableCollection<CustomerDto> FilteredCustomers { get; private set; }
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

        private async void OnUpdateCustomer(CustomerDto selectedCustomer)
        {
            try
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

        private async void OnDeleteCustomer(CustomerDto selectedCustomer)
        {
            try
            {
                bool isConfirm = await _dialogService.ShowConfirmation();

                if (!isConfirm) return;
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

        private async void OnShowDetails(CustomerDto customerDto)
        {
            var view = new CustomerDetailsForm()
            {
                DataContext = new CustomerFormViewModel(customerDto)
            };

            await DialogHost.Show(view, "RootDialog");
        }

        #endregion
    }
}
