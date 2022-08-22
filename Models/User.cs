namespace DrugMMvc.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Loc { get; set; }
        public string? UserPassword { get; set; }
    }
}
