using RefactoringChallenge.Dal.Abstractions;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;

namespace RefactoringChallenge.Services;

public class OrderService(IRepository<Order> repository, IDiscountService discountService) : IOrderService
{
    /// <inheritdoc />
    public Task<List<Order>> GetCustomerPendingOrdersAsync(int customerId) =>
        repository.ItemsAsync(o => o.CustomerId == customerId && o.Status == "Pending");

    public Task SaveAsync(Order order) => repository.SaveAsync(order);

    /// <inheritdoc />
    public Task Process(Order order, Customer customer)
    {
        var totalAmount = CountTotalAmount(order);
        var discountPercent = discountService.GetDiscount(customer, totalAmount);

        var discountAmount = totalAmount * (discountPercent / 100);
        var finalAmount = totalAmount - discountAmount;
                
        order.DiscountPercent = discountPercent;
        order.DiscountAmount = discountAmount;
        order.TotalAmount = finalAmount;
        order.Status = "Processed";
        
        return SaveAsync(order);
    }
    
    private static decimal CountTotalAmount(Order order) =>
        order.Items.Sum(orderItem => orderItem.Quantity * orderItem.UnitPrice);
}
