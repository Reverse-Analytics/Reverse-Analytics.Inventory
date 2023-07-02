using Inventory.Core.Enums;
using System;

namespace Inventory.Core.Models
{
    public class SaleDebt
    {
        public int Id { get; set; }
        public decimal TotalDue { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DebtStatus Status { get; set; }

        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
