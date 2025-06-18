using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RefactoringChallenge.Dal.Extensions;

public static class DalExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<RefactoringChallengeDbContext>((_, options) => options.UseSqlServer(connectionString),
            optionsLifetime: ServiceLifetime.Singleton);
}
