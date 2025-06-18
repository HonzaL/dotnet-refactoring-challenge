using Moq;
using RefactoringChallenge.Dal.Abstractions;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;

namespace RefactoringChallenge.Services.Tests;

[TestFixture]
public class OrderServiceTests
{
    private OrderService? _orderService;
    private Mock<IRepository<Order>>? _repository;
    private Mock<IDiscountService>? _discountService;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IRepository<Order>>();
        _discountService = new Mock<IDiscountService>();
        _orderService = new OrderService(_repository.Object, _discountService.Object);
    }

    [Test]
    public async Task Process_ShouldCountWithoutDiscount()
    {
        // arrange
        var order = new Order
        {
            Items =
            {
                new OrderItem { Quantity = 5, UnitPrice = (decimal)15.50 }
            }
        };
        
        // act
        await _orderService!.Process(order,  new Customer());
        
        // assert
        Assert.That(order.Status, Is.EqualTo("Processed"));
        Assert.That(order.TotalAmount, Is.EqualTo(77.50));
        Assert.That(order.DiscountPercent, Is.EqualTo(0));
        Assert.That(order.DiscountAmount, Is.EqualTo(0));
        _repository!.Verify(x => x.SaveAsync(It.IsAny<Order>()), Times.Exactly(1));
    }

    [Test]
    public async Task Process_ShouldCountWithDiscount()
    {
        // arrange
        var order = new Order
        {
            Items =
            {
                new OrderItem { Quantity = 10, UnitPrice = 20 }
            }
        };
        _discountService!
            .Setup(x => x.GetDiscount(It.IsAny<Customer>(), It.IsAny<decimal>()))
            .Returns(20);
        
        // act
        await _orderService!.Process(order,  new Customer());
        
        // assert
        Assert.That(order.Status, Is.EqualTo("Processed"));
        Assert.That(order.TotalAmount, Is.EqualTo(160));
        Assert.That(order.DiscountPercent, Is.EqualTo(20));
        Assert.That(order.DiscountAmount, Is.EqualTo(40));
        _repository!.Verify(x => x.SaveAsync(It.IsAny<Order>()), Times.Exactly(1));
    }
}
