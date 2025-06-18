using Microsoft.Extensions.DependencyInjection;
using RefactoringChallenge.Services.Abstractions;
using RefactoringChallenge.Services.Abstractions.Providers;
using RefactoringChallenge.Services.Abstractions.Resolvers;
using RefactoringChallenge.Services.Processor;
using RefactoringChallenge.Services.Providers;

namespace RefactoringChallenge.Services.Extensions;

public static class ServiceExtensions
{
    private static IServiceCollection AddResolvers(this IServiceCollection services)
    {
        var discountResolverTypes = typeof(ServiceExtensions).Assembly.GetTypes()
            .Where(t => typeof(IDiscountResolver).IsAssignableFrom(t));
        foreach (var discountResolverType in discountResolverTypes)
        {
            services.AddSingleton(typeof(IDiscountResolver), discountResolverType);
        }
        
        return services;
    }
    
    private static IServiceCollection AddProviders(this IServiceCollection services) =>
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    
    private static IServiceCollection AddBaseServices(this IServiceCollection services) =>
        services
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IInventoryService, InventoryService>()
            .AddScoped<IOrderLogService, OrderLogService>()
            .AddSingleton<IDiscountService, DiscountService>()
        ;

    private static void AddProcessors(this IServiceCollection services) =>
        services.AddScoped<CustomerOrderProcessor>();

    public static void AddServices(this IServiceCollection services) =>
        services
            .AddResolvers()
            .AddProviders()
            .AddBaseServices()
            .AddProcessors();

}
