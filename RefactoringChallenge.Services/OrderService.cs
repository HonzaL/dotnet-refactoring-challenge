using RefactoringChallenge.Dal.Abstractions;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;

namespace RefactoringChallenge.Services;

public class OrderService(IRepository<Order> repository) : IOrderService
{
    public Task<List<Order>> GetCustomerPendingOrdersAsync(int customerId) =>
        repository.ItemsAsync(o => o.CustomerId == customerId && o.Status == "Pending");

    public Task SaveAsync(Order order) => repository.SaveAsync(order);
}
