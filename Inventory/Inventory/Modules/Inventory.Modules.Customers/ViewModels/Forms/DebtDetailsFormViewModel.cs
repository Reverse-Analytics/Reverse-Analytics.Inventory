using Inventory.Core;
using Inventory.Core.Models;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace Inventory.Modules.Customers.ViewModels.Forms
{
    public class DebtDetailsFormViewModel
    {
        public string Customer { get; set; }
        public string Salesman { get; set; }
        public string Status { get; set; }
        public DateTime DebtDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Leftover { get; set; }

        public ICommand CloseCommand { get; }

        public DebtDetailsFormViewModel(SaleDebt debt)
        {
            ArgumentNullException.ThrowIfNull(debt);

            Customer = "John Doe";
            Salesman = "Smith Johnson";
            Status = "Paid";
            DueDate = debt.DueDate;
            TotalAmount = debt.TotalDue;
            Leftover = debt.TotalDue;
            Status = debt.Status.ToString();

            CloseCommand = new DelegateCommand(OnClose);
        }

        public void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
