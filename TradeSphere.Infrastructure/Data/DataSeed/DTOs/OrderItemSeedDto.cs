namespace TradeSphere.Infrastructure.Data.DataSeed.DTOs
{
    public class OrderItemSeedDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UserEmail { get; set; }
        public string ProductName { get; set; }
    }
}