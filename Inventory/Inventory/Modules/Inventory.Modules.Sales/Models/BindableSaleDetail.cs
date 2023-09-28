using Prism.Mvvm;

namespace Inventory.Modules.Sales.Models
{
    public class BindableSaleDetail : BindableBase
    {
        private bool _unitPriceUpdating;
        private bool _quantityUpdating;
        private bool _discountPercentageUpdating;


        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }

        public decimal SalePrice { get; set; }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPriceUpdating) return;

                if (value != _unitPrice && value > 0)
                {
                    _unitPriceUpdating = true;
                    SetProperty(ref _unitPrice, value);
                    UnitPriceUpdated();
                    _unitPriceUpdating = false;
                }
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantityUpdating) return;

                if (value != _quantity && value > 0)
                {
                    _quantityUpdating = true;
                    SetProperty(ref _quantity, value);
                    QuantityUpdated();
                    _quantityUpdating = false;
                }
            }
        }

        private decimal _discountPercentage;
        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (_discountPercentageUpdating) return;

                if (value != _discountPercentage && value > 0)
                {
                    _discountPercentageUpdating = true;
                    SetProperty(ref _discountPercentage, value);
                    DiscountPercentageUpdated();
                    _discountPercentageUpdating = false;
                }
            }
        }

        private decimal _totalDiscount;
        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set => SetProperty(ref _totalDiscount, value);
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (value != _totalPrice)
                {
                    SetProperty(ref _totalPrice, value);
                }
            }
        }

        private void UnitPriceUpdated()
        {
            if (UnitPrice < SalePrice)
            {
                DiscountPercentage = 100 - (UnitPrice * 100 / SalePrice);
                TotalDiscount = (SalePrice - UnitPrice) * Quantity;
            }
            else
            {
                DiscountPercentage = 0;
                TotalDiscount = 0;
            }

            TotalPrice = UnitPrice * Quantity;
        }

        private void QuantityUpdated()
        {
            TotalPrice = UnitPrice * Quantity;
            TotalDiscount = (SalePrice - UnitPrice) * Quantity;
        }

        private void DiscountPercentageUpdated()
        {
            UnitPrice = SalePrice * (1 - (DiscountPercentage / 100));
            TotalPrice = UnitPrice * Quantity;
            TotalDiscount = (SalePrice - UnitPrice) * Quantity;
        }
    }
}
