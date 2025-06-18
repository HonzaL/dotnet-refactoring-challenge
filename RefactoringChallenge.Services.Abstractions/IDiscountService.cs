using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions;

public interface IDiscountService
{
    /// <summary>
    /// Discount service - compute discount for given Customer and order amount
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="totalAmount">Order amount</param>
    /// <returns>Discount in percent</returns>
    decimal GetDiscount(Customer customer, decimal totalAmount);
}
