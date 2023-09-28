using System;
using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class Refund
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime RefundDate { get; set; }
        public string? Reason { get; set; }
        public string? ReceivedBy { get; set; }

        public string Receipt
        {
            get => Sale?.Receipt ?? "";
        }
        public string Customer
        {
            get => Sale?.Customer?.FullName ?? "";
        }

        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; }

        public ICollection<RefundDetail> RefundDetails { get; set; }
    }
}
