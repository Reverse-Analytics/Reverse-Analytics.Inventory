using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class CustomerDetailsFormViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double Discount { get; set; }
        public decimal Balance { get; set; }

        public DelegateCommand CancelCommand { get; }

        public ObservableCollection<Sale> Sales { get; set; }

        public CustomerDetailsFormViewModel(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            Id = customer.Id;
            FullName = customer.FullName;
            CompanyName = customer.Company;
            PhoneNumber = customer.PhoneNumber;
            Address = customer.Address;
            Balance = customer.Balance;
            Discount = customer.Discount;

            Sales = new(customer.Sales);

            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnCancel()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
