using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions.Providers;
using RefactoringChallenge.Services.Abstractions.Resolvers;

namespace RefactoringChallenge.Services.Resolvers;

/// <summary>
/// Customer Discount resolver - Calculates the discount applicable to a given customer
/// </summary>
public class CustomerDiscountResolver(IDateTimeProvider dateTimeProvider) : IDiscountResolver
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
        switch (yearsAsCustomer)
        {
            case >= 5:
                discountPercent += 5;
                break;
            case >= 2:
                discountPercent += 2;
                break;
        }
        return  discountPercent;
    }
}
