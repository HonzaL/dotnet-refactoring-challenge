using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions;

public interface IOrderService
{
    /// <summary>
    /// Returns all pending orders for a given customer
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <returns>Customer's pending orders</returns>
    Task<List<Order>> GetCustomerPendingOrdersAsync(int customerId);
    Task SaveAsync(Order order);
    /// <summary>
    /// Order processing - fills in the aggregated data and saves
    /// </summary>
    /// <param name="order">Order</param>
    /// <param name="customer">Customer</param>
    Task Process(Order order, Customer customer);
}
