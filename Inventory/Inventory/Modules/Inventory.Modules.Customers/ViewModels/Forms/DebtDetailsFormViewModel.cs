using Inventory.Core;
using Inventory.Core.Enums;
using Inventory.Core.Models;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class DebtDetailsFormViewModel
    {
        public string Receipt { get; set; }
        public string Customer { get; set; }
        public string Salesman { get; set; }
        public DebtStatus Status { get; set; }
        public DateTime DebtDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string TotalAmount { get; set; }
        public string Leftover { get; set; }

        public ICommand CloseCommand { get; }

        public DebtDetailsFormViewModel(SaleDebt debt)
        {
            ArgumentNullException.ThrowIfNull(debt);

            Receipt = debt.Receipt;
            Customer = debt.Customer;
            Salesman = debt.SoldBy;
            Status = debt.Status;
            DebtDate = debt.DebtDate;
            ClosedDate = debt.ClosedDate;
            TotalAmount = debt.TotalDue.ToString("N");
            Leftover = (debt.TotalDue - debt.TotalPaid).ToString("N");

            CloseCommand = new DelegateCommand(OnClose);
        }

        public void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
