namespace TradeSphere.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation properties
        public ShoppingCart ShoppingCart { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<FeedBack> FeedBacks { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}