using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;
using RefactoringChallenge.Services.Abstractions.Resolvers;

namespace RefactoringChallenge.Services;

public class DiscountService(IEnumerable<IDiscountResolver> discountResolvers) : IDiscountService
{
    /// <inheritdoc />
    public decimal GetDiscount(Customer customer, decimal totalAmount)
    {
        var discountPercent = discountResolvers.Select(x => x.GetDiscount(customer, totalAmount)).Sum();
        
        if (discountPercent > 25)
        {
            discountPercent = 25;
        }
        
        return discountPercent;
    }
}
