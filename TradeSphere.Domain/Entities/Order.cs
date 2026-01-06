namespace TradeSphere.Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int ApplicationUserId { get; set; }

        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; }
        public Payment Payment { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
