using RefactoringChallenge.Dal.Abstractions;
using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;

namespace RefactoringChallenge.Services;

public class CustomerService(IRepository<Customer> repository) : ICustomerService
{
    /// <inheritdoc />
    public Task<Customer?> FindByIdAsync(int customerId) => repository.FindAsync(customerId);
}
