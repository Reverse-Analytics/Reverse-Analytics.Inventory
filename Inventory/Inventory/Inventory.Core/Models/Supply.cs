using System;
using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class Supply
    {
        public string? ReceivedBy { get; set; }
        public string? Comment { get; set; }
        public decimal TotalDue { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime SupplyDate { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<SupplyDetail> SupplyDetails { get; set; }
        public virtual ICollection<SupplyDebt> SupplyDebts { get; set; }
    }
}
