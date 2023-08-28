using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Inventory.Modules.Sales.ViewModels.Forms
{
    public class SaleDetailsFormViewModel : ViewModelBase
    {
        public string CustomerFullName { get; set; }
        public DateTime SaleDate { get; set; }
        public string Salesman { get; set; }
        public decimal TotalDue { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DebtAmount { get; set; }
        public string Receipt { get; set; }
        public string Comments { get; set; }
        public List<Inventory.Core.Models.SaleDetail> SaleDetails { get; }

        public ICommand CloseCommand { get; }

        public SaleDetailsFormViewModel(Sale sale)
        {
            CustomerFullName = sale.Customer.FullName;
            SaleDate = sale.SaleDate;
            Salesman = sale.Customer.FullName;
            TotalDue = sale.TotalDue;
            Discount = sale.TotalDiscount;
            PaidAmount = sale.TotalPaid;
            DebtAmount = sale.TotalDue - sale.TotalPaid;
            Receipt = sale.Receipt;
            Comments = sale.Comments;
            SaleDetails = sale.SaleDetails.ToList();

            CloseCommand = new DelegateCommand(OnClose);
        }

        private void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
