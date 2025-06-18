using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Dal;
using RefactoringChallenge.Services.Abstractions;

namespace RefactoringChallenge.Services;

public class InventoryService(RefactoringChallengeDbContext context) : IInventoryService
{
    /// <inheritdoc />
    public async Task<bool> IsAllOrderItemsInStockAsync(int orderId)
    {
        var outOfStockItemsCount = await context.Database
            .SqlQueryRaw<int?>(
                "SELECT COUNT(*) AS Value FROM OrderItems AS oi LEFT JOIN Inventory AS i ON oi.ProductId = i.ProductId WHERE OrderId = {0} AND i.StockQuantity < oi.Quantity",
                new SqlParameter("OrderId", orderId))
            .SingleOrDefaultAsync();
        return outOfStockItemsCount == 0;
    }

    /// <inheritdoc />
    public Task RemoveOrderItemsFromStockAsync(int orderId) =>
        context.Database.ExecuteSqlRawAsync(
            "UPDATE Inventory SET StockQuantity = StockQuantity - Quantity FROM Inventory INNER JOIN OrderItems ON OrderItems.ProductId = Inventory.ProductId WHERE OrderId = {0}",
            new SqlParameter("OrderId", orderId));
}
