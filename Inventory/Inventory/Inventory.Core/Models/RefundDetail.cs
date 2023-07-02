namespace Inventory.Core.Models
{
    public class RefundDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int RefundId { get; set; }
        public virtual Refund Refund { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
