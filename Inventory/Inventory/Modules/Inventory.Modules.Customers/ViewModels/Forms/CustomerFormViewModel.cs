using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using ReverseAnalytics.Domain.DTOs.CustomerPhoneDto;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class CustomerFormViewModel : BindableBase
    {
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

        private void OnCancel() => DialogHost.Close("RootDialog");

        private void OnSave()
        {
            var result = new CustomerForCreateDto()
            {
                FullName = FullName,
                CompanyName = CompanyName,
                ContactPersonPhone = PhoneNumber,
            };

            DialogHost.Close("RootDialog", result);
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
