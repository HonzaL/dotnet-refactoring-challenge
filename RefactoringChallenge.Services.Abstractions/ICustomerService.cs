using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Services.Abstractions;

public interface ICustomerService
{
    Task<Customer?> FindByIdAsync(int customerId);
}
