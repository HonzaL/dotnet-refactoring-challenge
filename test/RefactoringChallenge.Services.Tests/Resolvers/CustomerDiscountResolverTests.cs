using Moq;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions.Providers;
using RefactoringChallenge.Services.Resolvers;

namespace RefactoringChallenge.Services.Tests.Resolvers;

[TestFixture]
public class CustomerDiscountResolverTests
{
    private Mock<IDateTimeProvider>? _dateTimeProvider;
    private CustomerDiscountResolver? _discountResolver;
    
    [SetUp]
    public void SetUp()
    {
        _dateTimeProvider = new Mock<IDateTimeProvider>();
        _discountResolver = new CustomerDiscountResolver(_dateTimeProvider.Object);
    }
    
    [Test]
    public void GetDiscount_ShouldReturn10Percent_VipCustomer()
    {
        // act
        var discount = _discountResolver!.GetDiscount(new Customer
        {
            RegistrationDate = DateTime.UtcNow,
            IsVip = true
        }, 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(10));
    }
    
    [Test]
    public void GetDiscount_ShouldReturn5PercentYearsAsCustomer()
    {
        // arrange
        _dateTimeProvider!.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2025, 6, 18, 1, 53, 12, TimeSpan.Zero));
        
        // act
        var discount = _discountResolver!.GetDiscount(new Customer
        {
            RegistrationDate = new DateTime(2018, 12, 24)
        }, 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(5));
    }

    [Test]
    public void GetDiscount_ShouldReturn2PercentYearsAsCustomer()
    {
        // arrange
        _dateTimeProvider!.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2025, 6, 18, 1, 53, 12, TimeSpan.Zero));
        
        // act
        var discount = _discountResolver!.GetDiscount(new Customer
        {
            RegistrationDate = new DateTime(2022, 12, 24)
        }, 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(2));
    }

    [Test]
    public void GetDiscount_ShouldReturnZero()
    {
        // act
        var discount = _discountResolver!.GetDiscount(new Customer(), 0);
        
        // assert
        Assert.That(discount, Is.EqualTo(0));
    }
}
