using Inventory.Core;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using ReverseAnalytics.Domain.DTOs.Debt;
using System.Windows.Input;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class DebtPaymentFormViewModel : ViewModelBase
    {
        public ICommand CompletePaymentCommand { get; }
        public ICommand CloseCommand { get; }

        public int SaleId { get; set; }
        public decimal TotalAmount { get; set; }

        private decimal _amountToPay;
        public decimal AmountToPay
        {
            get => _amountToPay;
            set
            {
                if (TotalAmount - value < 0) return;

                LeftOver = TotalAmount - value;
                SetProperty(ref _amountToPay, value);
            }
        }

        private decimal _leftOver;
        public decimal LeftOver
        {
            get => _leftOver;
            set => SetProperty(ref _leftOver, value);
        }

        public DebtPaymentFormViewModel(DebtDto debt)
        {
            SaleId = debt.Id;
            TotalAmount = debt.Amount;
            AmountToPay = 0;
            LeftOver = debt.Amount - 0;

            CloseCommand = new DelegateCommand(() => DialogHost.Close(RegionNames.DialogRegion));
        }
    }
}
