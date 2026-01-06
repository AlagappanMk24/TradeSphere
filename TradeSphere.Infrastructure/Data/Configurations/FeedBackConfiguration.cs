namespace TradeSphere.Infrastructure.Data.Configurations
{
    public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.ToTable("FeedBacks");
            builder.Property(f => f.CreatedAt).HasColumnType("datetime");
            builder.Property(f => f.Rating).IsRequired();
            builder.Property(f => f.Comment).IsRequired().HasMaxLength(1000);
            builder.HasOne(f => f.ApplicationUser).WithMany(a => a.FeedBacks).HasForeignKey(f => f.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(f => f.Product).WithMany(p => p.FeedBacks).HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(f => new { f.ApplicationUserId, f.ProductId })
                .IsUnique();
        }
    }
}