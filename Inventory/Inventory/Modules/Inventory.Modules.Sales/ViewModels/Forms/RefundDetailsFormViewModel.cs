using Inventory.Core;
using Inventory.Core.Models;
using Inventory.Core.Mvvm;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inventory.Modules.Sales.ViewModels.Forms
{
    internal class RefundDetailsFormViewModel : ViewModelBase
    {

        #region Services

        #endregion

        #region Commands

        public DelegateCommand CloseCommand { get; set; }

        #endregion

        #region Collections

        public ObservableCollection<RefundDetail> RefundItems { get; set; }

        #endregion

        #region Properties

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
        }

        private decimal _totalRefunded;
        public decimal TotalRefunded
        {
            get => _totalRefunded;
            set => SetProperty(ref _totalRefunded, value);
        }

        private decimal _debtCoverage;
        public decimal DebtCoverage
        {
            get => _debtCoverage;
            set => SetProperty(ref _debtCoverage, value);
        }

        private DateTime _refundDate;
        public DateTime RefundDate
        {
            get => _refundDate;
            set => SetProperty(ref _refundDate, value);
        }

        private string _receivedBy;
        public string ReceivedBy
        {
            get => _receivedBy;
            set => SetProperty(ref _receivedBy, value);
        }

        private int _saleId;
        public int SaleId
        {
            get => _saleId;
            set => SetProperty(ref _saleId, value);
        }

        private string _receipt;
        public string Receipt
        {
            get => _receipt;
            set => SetProperty(ref _receipt, value);
        }

        #endregion

        #region Constructors

        public RefundDetailsFormViewModel()
        {
            CloseCommand = new DelegateCommand(OnClose);

            RefundItems = new ObservableCollection<RefundDetail>();
        }

        public RefundDetailsFormViewModel(Refund refund)
            : this()
        {
            TotalAmount = refund.TotalAmount;
            TotalRefunded = refund.RefundAmount;
            DebtCoverage = refund.DebtPaymentAmount;
            RefundDate = refund.RefundDate;
            ReceivedBy = refund.ReceivedBy;
            SaleId = refund.SaleId;
            Receipt = refund.Sale?.Receipt;

            SetupCollection(refund);
        }

        #endregion

        #region Command Methods

        private void OnClose()
        {
            DialogHost.Close(RegionNames.DialogRegion);
        }

        #endregion

        #region Helper Methods

        private void SetupCollection(Refund refund)
        {
            foreach (var item in refund.RefundDetails)
            {
                var saleDetail = refund.Sale.SaleDetails.FirstOrDefault(x => x.ProductId == item.ProductId);

                if (saleDetail != null)
                {
                    var totalAmount = item.Quantity * saleDetail?.UnitPrice;
                    item.TotalAmount = totalAmount ?? 0;

                    RefundItems.Add(item);
                }
            }
        }

        #endregion
    }
}
