using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Dal.Configurations;

public class OrderCfg : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .IsRequired();
        builder.Property(order => order.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(order => order.DiscountPercent).HasColumnType("decimal(5,2)").IsRequired(false);
        builder.Property(order => order.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired(false);
    }
}
