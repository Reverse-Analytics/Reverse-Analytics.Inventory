using Inventory.Core.Enums;
using System;

namespace Inventory.Core.Models
{
    public class SupplyDebt
    {
        public int Id { get; set; }
        public decimal TotalDue { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DebtStatus Status { get; set; }

        public int SupplyId { get; set; }
        public virtual Supply Supply { get; set; }
    }
}
