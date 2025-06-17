using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Dal;

public class RefactoringChallengeDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RefactoringChallengeDbContext).Assembly);
    }
}
