using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;
using System;
using System.Linq;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class CustomerFormViewModel : ViewModelBase
    {
        private readonly bool isEditingMode;

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public string FullName { get; set; }
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

            FullName = customer.FullName;
            CompanyName = customer.CompanyName;
            Address = customer.Addresses.Select(x => x.AddressDetails).FirstOrDefault();
            PhoneNumber = customer.Phones.FirstOrDefault(x => x.IsPrimary == true)?.PhoneNumber;
            Balance = customer.Balance ?? 0;
            Discount = 0;
        }

        private void OnCancel() => DialogHost.Close("RootDialog");

        private void OnSave()
        {
            if (isEditingMode)
            {
                var updatedCustomer = new CustomerForUpdateDto()
                {
                    FullName = FullName,
                    CompanyName = CompanyName,
                };

                DialogHost.Close("RootDialog", updatedCustomer);
            }
            else
            {
                var newCustomer = new CustomerForCreateDto()
                {
                    FullName = FullName,
                    CompanyName = CompanyName,
                    ContactPersonPhone = PhoneNumber,
                };

                DialogHost.Close("RootDialog", newCustomer);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(FullName) &&
                !string.IsNullOrEmpty(CompanyName) &&
                !string.IsNullOrEmpty(Address) &&
                !string.IsNullOrEmpty(PhoneNumber);
        }
    }
}
