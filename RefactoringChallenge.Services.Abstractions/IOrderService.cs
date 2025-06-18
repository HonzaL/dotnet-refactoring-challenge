using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions;

public interface IOrderService
{
    Task<List<Order>> GetCustomerPendingOrdersAsync(int customerId);
    Task SaveAsync(Order order);
}
