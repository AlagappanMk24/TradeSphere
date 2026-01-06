namespace TradeSphere.Domain.Entities
{
    public class FeedBack : BaseEntity
    {
        public int ApplicationUserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }

        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }
        public Product Product { get; set; }

    }
}
