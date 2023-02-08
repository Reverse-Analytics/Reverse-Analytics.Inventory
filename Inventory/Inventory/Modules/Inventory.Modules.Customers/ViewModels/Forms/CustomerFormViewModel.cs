using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using ReverseAnalytics.Domain.DTOs.Customer;
using System;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class CustomerFormViewModel : ViewModelBase
    {
        private readonly bool isEditingMode;

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public int Id { get; set; }
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set
            {
                SetProperty(ref _fullName, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public double Discount { get; set; }

        public CustomerFormViewModel()
        {
            CancelCommand = new DelegateCommand(OnCancel);
            SaveCommand = new DelegateCommand(OnSave, CanSave);

            Title = "Add Customer";
        }

        public CustomerFormViewModel(CustomerDto customer)
            : this()
        {
            isEditingMode = true;

            InitializeProperties(customer);

            Title = "Update Customer";
        }

        private void InitializeProperties(CustomerDto customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            Id = customer.Id;
            FullName = customer.FullName;
            CompanyName = customer.CompanyName;
            Address = customer.Address;
            PhoneNumber = customer.PhoneNumber;
            Balance = customer.Balance;
            Discount = 0;
        }

        private void OnCancel() => DialogHost.Close("RootDialog");

        private void OnSave()
        {
            if (isEditingMode)
            {
                var updatedCustomer = new CustomerForUpdateDto()
                {
                    Id = Id,
                    FullName = FullName,
                    CompanyName = CompanyName,
                    Address = Address,
                    PhoneNumber = PhoneNumber,
                    Balance = Balance,
                    Discount = Discount
                };

                DialogHost.Close("RootDialog", updatedCustomer);
            }
            else
            {
                var newCustomer = new CustomerForCreateDto()
                {
                    FullName = FullName,
                    CompanyName = CompanyName,
                    Address = Address,
                    PhoneNumber = PhoneNumber,
                    Balance = Balance,
                    Discount = Discount
                };

                DialogHost.Close("RootDialog", newCustomer);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(FullName);
        }
    }
}
