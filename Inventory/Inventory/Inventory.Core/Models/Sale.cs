﻿using Inventory.Core.Enums;
using System;
using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string Receipt { get; set; }
        public string? Comments { get; set; }
        public string? SoldBy { get; set; }
        public decimal TotalDue { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDiscount { get; set; }
        public double TotalDiscountPercentage { get; set; }
        public DateTime SaleDate { get; set; }
        public SaleType SaleType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int SaleDebtId { get; set; }
        public virtual SaleDebt SaleDebt { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        public virtual ICollection<Refund> Refunds { get; set; }
    }
}
