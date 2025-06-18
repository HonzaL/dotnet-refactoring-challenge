using Moq;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions.Providers;
using RefactoringChallenge.Services.Resolvers;

namespace RefactoringChallenge.Services.Tests.Resolvers;

[TestFixture]
public class DiscountResolverTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;
    private readonly DiscountResolver _discountResolver;

    public DiscountResolverTests()
    {
        _dateTimeProvider = new Mock<IDateTimeProvider>();
        _discountResolver = new DiscountResolver(_dateTimeProvider.Object);
    }

    [Test]
    public void GetDiscount_ShouldReturnUpperBoundary()
    {
        // act
        var discount = _discountResolver.GetDiscount(new Customer
        {
            IsVip = true
        }, 1_000_000);
        
        // assert
        Assert.That(discount, Is.EqualTo(25));
    }

    [Test]
    public void GetDiscount_ShouldReturn5PercentYearsAsCustomer()
    {
        // assert
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2025, 6, 18, 1, 53, 12, TimeSpan.Zero));
        
        // act
        var discount = _discountResolver.GetDiscount(new Customer
        {
            RegistrationDate = new DateTime(2018, 12, 24)
        }, 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(5));
    }

    [Test]
    public void GetDiscount_ShouldReturn2PercentYearsAsCustomer()
    {
        // assert
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2025, 6, 18, 1, 53, 12, TimeSpan.Zero));
        
        // act
        var discount = _discountResolver.GetDiscount(new Customer
        {
            RegistrationDate = new DateTime(2022, 12, 24)
        }, 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(2));
    }

    [Test]
    public void GetDiscount_ShouldReturn10PercentAmount()
    {
        // act
        var discount = _discountResolver.GetDiscount(new Customer
        {
            RegistrationDate = new DateTime(2024, 12, 24)
        }, 5001);
        
        // assert
        Assert.That(discount, Is.EqualTo(10));
    }
}
