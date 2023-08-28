using Inventory.Core.Enums;
using System;

namespace Inventory.Core.Models
{
    public class SaleDebt
    {
        public int Id { get; set; }
        public decimal TotalDue { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime DebtDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DebtStatus Status { get; set; }
        public string Receipt { get; set; }
        public string Customer { get; set; }
        public string SoldBy { get; set; }
    }
}
