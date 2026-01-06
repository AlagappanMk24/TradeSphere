namespace TradeSphere.Domain.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public int ApplicationUserId { get; set; }

        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}