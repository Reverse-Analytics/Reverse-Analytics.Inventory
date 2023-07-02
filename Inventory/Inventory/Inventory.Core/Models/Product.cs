﻿using Inventory.Core.Enums;
using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string? Description { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? SupplyPrice { get; set; }
        public double QuantityInStock { get; set; }
        public double? Volume { get; set; }
        public double? Weight { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        public int CategoryId { get; set; }
        public virtual ProductCategory Category { get; set; }

        public virtual ICollection<SaleDetail> SaleDetails { get; set; }
        public virtual ICollection<SupplyDetail> PurchaseDetails { get; set; }
        public virtual ICollection<RefundDetail> RefundDetails { get; set; }
    }
}
