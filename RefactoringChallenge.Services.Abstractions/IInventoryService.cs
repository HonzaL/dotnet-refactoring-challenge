namespace RefactoringChallenge.Services.Abstractions;

public interface IInventoryService
{
    /// <summary>
    /// Check if all items from the order are in stock
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>True if all items are in stock, otherwise false</returns>
    Task<bool> IsAllOrderItemsInStockAsync(int orderId);
    /// <summary>
    /// Removes all items from the given order from inventory
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    Task RemoveOrderItemsFromStockAsync(int orderId);
}
