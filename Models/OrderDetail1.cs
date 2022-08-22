namespace DrugMMvc.Models
{
    public class OrderDetail1
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
