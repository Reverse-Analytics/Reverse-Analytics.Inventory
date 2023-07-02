using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Company { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Supply> Supplies { get; set; }
    }
}
