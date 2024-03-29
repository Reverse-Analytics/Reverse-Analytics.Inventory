﻿using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Linq;

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

        private string _phoneNumber = "+998 ";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (ValidatePhoneNumber(value))
                {
                    return;
                }

                SetProperty(ref _phoneNumber, value);
            }
        }
        public decimal Balance { get; set; }
        public double Discount { get; set; }

        public CustomerFormViewModel()
        {
            CancelCommand = new DelegateCommand(OnCancel);
            SaveCommand = new DelegateCommand(OnSave, CanSave);

            Title = "Add Customer";
        }

        public CustomerFormViewModel(Customer customer)
            : this()
        {
            isEditingMode = true;

            InitializeProperties(customer);

            Title = "Update Customer";
        }

        private void InitializeProperties(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            Id = customer.Id;
            FullName = customer.FullName;
            CompanyName = customer.Company;
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
                var updatedCustomer = new Customer()
                {
                    Id = Id,
                    FullName = FullName,
                    Company = CompanyName,
                    Address = Address,
                    PhoneNumber = PhoneNumber,
                    Balance = Balance,
                    Discount = Discount
                };

                DialogHost.Close("RootDialog", updatedCustomer);
            }
            else
            {
                var newCustomer = new Customer()
                {
                    FullName = FullName,
                    Company = CompanyName,
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

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            if (phoneNumber.Length < 5 || phoneNumber.Length > 14)
            {
                return false;
            }

            return phoneNumber.Any(el => !char.IsDigit(el) && el != ' ' && el != '+');
        }
    }
}
