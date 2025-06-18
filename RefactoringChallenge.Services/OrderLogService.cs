using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Dal;
using RefactoringChallenge.Services.Abstractions;
using RefactoringChallenge.Services.Abstractions.Providers;

namespace RefactoringChallenge.Services;

public class OrderLogService(RefactoringChallengeDbContext context, IDateTimeProvider dateTimeProvider) : IOrderLogService
{
    /// <inheritdoc />
    public Task AddAsync(int orderId, string message) =>
        context.Database.ExecuteSqlRawAsync(
            "INSERT INTO OrderLogs (OrderId, LogDate, Message) VALUES ({0}, {1}, {2})",
            new SqlParameter("OrderId", orderId), 
            new SqlParameter("LogDate", dateTimeProvider.UtcNow.Date),
            new SqlParameter("Message", message));
}
