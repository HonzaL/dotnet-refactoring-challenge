using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Dal.Configurations;

public class OrderItemsCfg : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", "dbo");
        builder.Navigation(x => x.Product).AutoInclude();
    }
}
