namespace RefactoringChallenge.Services.Abstractions.Providers;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
