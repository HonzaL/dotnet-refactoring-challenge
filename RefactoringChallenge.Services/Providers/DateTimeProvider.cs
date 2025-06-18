using RefactoringChallenge.Services.Abstractions.Providers;

namespace RefactoringChallenge.Services.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
