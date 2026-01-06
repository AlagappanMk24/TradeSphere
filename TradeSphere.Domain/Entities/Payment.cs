namespace TradeSphere.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public int ApplicationUserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime PaymentDate { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
