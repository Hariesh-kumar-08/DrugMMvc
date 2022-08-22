using System.ComponentModel.DataAnnotations.Schema;

namespace DrugMMvc.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? PurchaseId { get; set; }
        public int? Quantity { get; set; }

        [NotMapped]

        public string? ProductName { get; set; }

        [NotMapped]

        public int? UnitPrice { get; set; }



        public virtual Product? Product { get; set; }
        public virtual Buyer? Purchase { get; set; }
        public virtual User? User { get; set; }
    }
}
