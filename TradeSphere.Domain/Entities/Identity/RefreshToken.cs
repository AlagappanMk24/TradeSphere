namespace TradeSphere.Domain.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public int AppUserId { get; set; }
        public bool RememberMe { get; set; }
        // Navigation Property
        public ApplicationUser AppUser { get; set; }
    }
}