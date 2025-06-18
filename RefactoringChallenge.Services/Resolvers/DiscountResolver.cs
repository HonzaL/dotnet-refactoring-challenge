using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions.Providers;
using RefactoringChallenge.Services.Abstractions.Resolvers;

namespace RefactoringChallenge.Services.Resolvers;

public class DiscountResolver(IDateTimeProvider dateTimeProvider) : IDiscountResolver
{
    /// <inheritdoc />
    public decimal GetDiscount(Customer customer, decimal totalAmount)
    {
        decimal discountPercent = 0;

        if (customer.IsVip)
        {
            discountPercent += 10;
        }

        var yearsAsCustomer = dateTimeProvider.UtcNow.Year - customer.RegistrationDate.Year;
        if (yearsAsCustomer >= 5)
        {
            discountPercent += 5;
        }
        else if (yearsAsCustomer >= 2)
        {
            discountPercent += 2;
        }

        if (totalAmount > 10000)
        {
            discountPercent += 15;
        }
        else if (totalAmount > 5000)
        {
            discountPercent += 10;
        }
        else if (totalAmount > 1000)
        {
            discountPercent += 5;
        }

        if (discountPercent > 25)
        {
            discountPercent = 25;
        }

        return discountPercent;
    }
}
