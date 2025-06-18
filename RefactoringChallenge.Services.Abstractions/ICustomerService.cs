using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions;

public interface ICustomerService
{
    /// <summary>
    /// Finds a Customer by identifier
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <returns>Customer or null if customer is missing</returns>
    Task<Customer?> FindByIdAsync(int customerId);
}
