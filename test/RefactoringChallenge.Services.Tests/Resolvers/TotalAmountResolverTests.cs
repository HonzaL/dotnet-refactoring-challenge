using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Resolvers;

namespace RefactoringChallenge.Services.Tests.Resolvers;

[TestFixture]
public class TotalAmountResolverTests
{
    private readonly TotalAmountDiscountResolver _discountResolver = new();

    [Test]
    [TestCase(100001, 15)]
    [TestCase(10000, 10)]
    [TestCase(5001, 10)]
    [TestCase(5000, 5)]
    [TestCase(1001, 5)]
    [TestCase(1000, 0)]
    [TestCase(1, 0)]
    [TestCase(0, 0)]
    [TestCase(-15, 0)]
    public void GetDiscount_ShouldReturnExpectedPercentAmount(decimal totalAmount, decimal expected)
    {
        // act
        var discount = _discountResolver.GetDiscount(new Customer(), totalAmount);
        
        // assert
        Assert.That(discount, Is.EqualTo(expected));
    }
}
