using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace TradeSphere.Infrastructure.Data.DbContext
{
    public class TradeSphereDbContext(DbContextOptions<TradeSphereDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options)
    {

        #region Db Tables
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(TradeSphereDbContext).Assembly);
        }
    }
}