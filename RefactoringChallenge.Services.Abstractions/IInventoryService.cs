namespace RefactoringChallenge.Services.Abstractions;

public interface IInventoryService
{
    Task<bool> IsAllOrderItemsInStockAsync(int orderId);
    Task RemoveOrderItemsFromStockAsync(int orderId);
}
