using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;
using ReverseAnalytics.Domain.Entities;
using System.Linq;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class CustomerFormViewModel : BindableBase
    {
        private readonly bool isEditingMode;

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set;  }

        private string _fullName;
        public string FullName 
        { 
            get => _fullName;
            set
            {
                _fullName = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _address;
        public string Address 
        {
            get => _address;
            set
            {
                _address = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber 
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private double _discount;
        public double Discount 
        {
            get => _discount;
            set
            {
                _discount = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public CustomerFormViewModel()
        {
            CancelCommand = new DelegateCommand(OnCancel);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
        }

        public CustomerFormViewModel(CustomerDto customer)
            : this()
        {
            isEditingMode = true;

            InitializeProperties(customer);
        }

        private void InitializeProperties(CustomerDto customer)
        {
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
            }

            var newCustomer = new CustomerForCreateDto()
            {
                FullName = FullName,
                CompanyName = CompanyName,
                ContactPersonPhone = PhoneNumber,
            };

            DialogHost.Close("RootDialog", newCustomer);
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
