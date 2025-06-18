using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions;

public interface IDiscountService
{
    /// <summary>
    /// Compute discount for the given Customer and order amount
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="totalAmount">Order amount</param>
    /// <returns>Discount in percent</returns>
    decimal GetDiscount(Customer customer, decimal totalAmount);
}
