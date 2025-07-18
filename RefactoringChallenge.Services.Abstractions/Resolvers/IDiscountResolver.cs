using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions.Resolvers;

public interface IDiscountResolver
{
    /// <summary>
    /// Compute discount
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="totalAmount">Total Order amount</param>
    /// <returns>Discount in percent</returns>
    decimal GetDiscount(Customer customer, decimal totalAmount);
}