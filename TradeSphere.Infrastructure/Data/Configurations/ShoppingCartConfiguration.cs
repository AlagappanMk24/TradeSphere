namespace TradeSphere.Infrastructure.Data.Configurations
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable("ShoppingCarts");
            builder.HasOne(s => s.ApplicationUser).WithOne(a => a.ShoppingCart).HasForeignKey<ShoppingCart>(s => s.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.CartItems).WithOne(c => c.ShoppingCart).HasForeignKey(c => c.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}