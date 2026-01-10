namespace TradeSphere.Infrastructure.Data.DataSeed.DTOs
{
    public class FeedBackSeedDto
    {
        public string UserEmail { get; set; }
        public string ProductName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}