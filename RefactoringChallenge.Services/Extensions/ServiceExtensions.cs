using Microsoft.Extensions.DependencyInjection;
using RefactoringChallenge.Services.Abstractions.Providers;
using RefactoringChallenge.Services.Abstractions.Resolvers;
using RefactoringChallenge.Services.Providers;
using RefactoringChallenge.Services.Resolvers;

namespace RefactoringChallenge.Services.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddResolvers(this IServiceCollection services) =>
        services.AddSingleton<IDiscountResolver, DiscountResolver>();
    
    public static void AddProviders(this IServiceCollection services) =>
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
}
