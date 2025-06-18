using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions.Resolvers;

namespace RefactoringChallenge.Services.Resolvers;

public class TotalAmountDiscountResolver : IDiscountResolver
{
    public decimal GetDiscount(Customer customer, decimal totalAmount)
    {
        decimal discountPercent = 0;
        
        switch (totalAmount)
        {
            case > 10000:
                discountPercent += 15;
                break;
            case > 5000:
                discountPercent += 10;
                break;
            case > 1000:
                discountPercent += 5;
                break;
        }

        return discountPercent;
    }
}
