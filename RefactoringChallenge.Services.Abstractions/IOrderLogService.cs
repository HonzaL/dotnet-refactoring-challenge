namespace RefactoringChallenge.Services.Abstractions;

public interface IOrderLogService
{
    Task AddAsync(int orderId, string message);
}
