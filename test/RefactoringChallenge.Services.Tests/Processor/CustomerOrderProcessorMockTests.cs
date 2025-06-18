using Moq;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;
using RefactoringChallenge.Services.Exceptions;
using RefactoringChallenge.Services.Processor;

namespace RefactoringChallenge.Services.Tests.Processor;

[TestFixture]
public class CustomerOrderProcessorMockTests
{
    private CustomerOrderProcessor? _processor;
    private Mock<ICustomerService>? _customerService;
    private Mock<IOrderService>? _orderService;

    [SetUp]
    public void Setup()
    {
        _customerService = new Mock<ICustomerService>();
        _orderService = new Mock<IOrderService>();
        _processor = new CustomerOrderProcessor(
            _customerService.Object,
            _orderService.Object,
            new Mock<IInventoryService>().Object,
            new Mock<IOrderLogService>().Object
        );
    }

    [Test]
    public void ProcessCustomerOrders_ShouldThrowException_NonExistentCustomer()
    {
        // arrange
        const int customerId = 0; // Non-existent customer

        // act
        var xe = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _processor!.ProcessCustomerOrdersAsync(customerId));
        
        // assert
        Assert.That(xe!.Message, Is.Not.Null);
        Assert.That(xe.Message, Does.StartWith("ID zákazníka musí být kladné číslo."));
    }

    [Test]
    public void ProcessCustomerOrders_ShouldThrowException_UnableToFindCustomer()
    {
        // arrange
        const int customerId = 15; // Non-existent customer

        // act
        var xe = Assert.ThrowsAsync<UnknownCustomerException>(async () =>
            await _processor!.ProcessCustomerOrdersAsync(customerId));
        
        // assert
        Assert.That(xe!.Message, Is.Not.Null);
        Assert.That(xe.CustomerId, Is.EqualTo(customerId));
    }

    [Test]
    public async Task ProcessCustomerOrders_ShouldReturnEmptyResult()
    {
        // arrange
        const int customerId = 15; // Non-existent customer
        _customerService!
            .Setup(x => x.FindByIdAsync(customerId))
            .ReturnsAsync(new Customer {  Id = customerId });
        _orderService!
            .Setup(x => x.GetCustomerPendingOrdersAsync(customerId))
            .ReturnsAsync([]);

        // act
        var result = await _processor!.ProcessCustomerOrdersAsync(customerId);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }
}
