namespace TradeSphere.Infrastructure.Data.DataSeed.DTOs
{
    public class OrderSeedDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string UserEmail { get; set; }
    }
}
