namespace TradeSphere.Infrastructure.Data.DataSeed.DTOs
{
    public class RefreshTokenSeedDto
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool RememberMe { get; set; }
    }
}
