namespace Inventory.Core.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
