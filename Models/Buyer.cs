using System.ComponentModel.DataAnnotations.Schema;

namespace DrugMMvc.Models
{
    public class Buyer
    {
        public int PurchaseId { get; set; }
        [NotMapped]

        public string? ProductName { get; set; }
        public int? TotalAmount { get; set; }
        public DateTime? DateofPurchase { get; set; }
        public string? PaymentMode { get; set; }
        public int? UserId { get; set; }

        //public virtual User? User { get; set; }
        //public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
