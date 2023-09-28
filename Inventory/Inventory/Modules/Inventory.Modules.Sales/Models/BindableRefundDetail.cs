using Prism.Mvvm;

namespace Inventory.Modules.Sales.Models
{
    public class BindableRefundDetail : BindableBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int SaleQuantity { get; set; }

        private int _refundQuantity;
        public int RefundQuantity
        {
            get => _refundQuantity;
            set
            {
                if (value > SaleQuantity)
                {
                    return;
                }

                SetProperty(ref _refundQuantity, value);
                TotalPriceAmount = RefundQuantity * UnitPrice;
            }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set => SetProperty(ref _unitPrice, value);
        }

        private decimal _totalPriceAmount;
        public decimal TotalPriceAmount
        {
            get => _totalPriceAmount;
            set => SetProperty(ref _totalPriceAmount, value);
        }
    }
}
