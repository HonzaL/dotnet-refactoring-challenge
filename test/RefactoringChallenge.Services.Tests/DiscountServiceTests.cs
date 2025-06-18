using Moq;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions.Resolvers;

namespace RefactoringChallenge.Services.Tests;

[TestFixture]
public class DiscountServiceTests
{
    private Mock<IDiscountResolver>? _discountResolver;
    private DiscountService? _discountService;

    [SetUp]
    public void SetUp()
    {
        _discountResolver = new Mock<IDiscountResolver>();
        _discountService = new DiscountService([_discountResolver.Object]);
    }

    [Test]
    public void GetDiscount_ShouldReturnUpperBoundary()
    {
        // arrange
        _discountResolver!
            .Setup(x => x.GetDiscount(It.IsAny<Customer>(), It.IsAny<decimal>()))
            .Returns(35);
        
        // act
        var discount = _discountService!.GetDiscount(new Customer(), 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(25));
    }

    [Test]
    public void GetDiscount_ShouldNotUseUpperBoundary()
    {
        // arrange
        var discountResolverDiscount = new decimal(17.5977);
        _discountResolver!
            .Setup(x => x.GetDiscount(It.IsAny<Customer>(), It.IsAny<decimal>()))
            .Returns(discountResolverDiscount);
        
        // act
        var discount = _discountService!.GetDiscount(new Customer(), 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(discountResolverDiscount));
    }
}
