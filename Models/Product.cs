namespace DrugMMvc.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? MfdDate { get; set; }
        public string? ExpDate { get; set; }
        public int? Price { get; set; }
        public int? Stock { get; set; }

      
    }
}
