using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System.Windows.Input;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class DebtPaymentFormViewModel : ViewModelBase
    {
        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        public int SaleId { get; set; }
        public decimal TotalAmount { get; set; }

        private decimal _amountToPay;
        public decimal AmountToPay
        {
            get => _amountToPay;
            set
            {
                if (TotalAmount - (value + TotalPaid) < 0)
                {
                    value = TotalAmount - TotalPaid;
                    SetProperty(ref _amountToPay, value);
                    LeftOver = 0;
                    return;
                }

                LeftOver = TotalAmount - (value + TotalPaid);
                SetProperty(ref _amountToPay, value);
            }
        }

        private decimal _totalPaid;
        public decimal TotalPaid
        {
            get => _totalPaid;
            set => SetProperty(ref _totalPaid, value);
        }

        private decimal _leftOver;
        public decimal LeftOver
        {
            get => _leftOver;
            set => SetProperty(ref _leftOver, value);
        }

        public DebtPaymentFormViewModel(SaleDebt debt)
        {
            SaleId = debt.Id;
            TotalAmount = debt.TotalDue;
            AmountToPay = 0;
            TotalPaid = debt.TotalPaid;
            LeftOver = (debt.TotalDue - debt.TotalPaid);

            CloseCommand = new DelegateCommand(() => DialogHost.Close("RootDialog"));
            SaveCommand = new DelegateCommand(OnSave);
        }

        private void OnSave()
        {
            var debt = new SaleDebt
            {
                TotalPaid = AmountToPay
            };

            DialogHost.Close("RootDialog", debt);
        }
    }
}
