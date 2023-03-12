using Inventory.Core;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using ReverseAnalytics.Domain.DTOs.Sale;
using ReverseAnalytics.Domain.DTOs.SaleDetail;
using ReverseAnalytics.Domain.Enums;
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
        public TransactionStatus Status { get; set; }
        public string Comments { get; set; }
        public List<SaleDetailDto> SaleDetails { get; }

        public ICommand CloseCommand { get; }

        public SaleDetailsFormViewModel(SaleDto sale)
        {
            CustomerFullName = sale.Customer.FullName;
            SaleDate = sale.SaleDate;
            Salesman = sale.Customer.FullName;
            TotalDue = sale.TotalDue;
            Discount = sale.DiscountTotal ?? 0;
            PaidAmount = sale.TotalPaid;
            DebtAmount = sale.TotalDue - sale.TotalPaid;
            Receipt = sale.Receipt;
            Status = sale.Status;
            Comments = sale.Comment;
            SaleDetails = sale.Details.ToList();

            CloseCommand = new DelegateCommand(OnClose);
        }

        private void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }
    }
}
