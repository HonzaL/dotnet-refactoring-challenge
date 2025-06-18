namespace RefactoringChallenge.Services.Abstractions;

public interface IOrderLogService
{
    /// <summary>
    /// Adds a new record
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <param name="message">Log message</param>
    Task AddAsync(int orderId, string message);
}
