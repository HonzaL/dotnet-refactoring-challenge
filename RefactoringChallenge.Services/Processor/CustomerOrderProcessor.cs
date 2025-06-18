using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Dal;
using RefactoringChallenge.Services.Abstractions.Resolvers;

namespace RefactoringChallenge.Services.Processor;

using Microsoft.Data.SqlClient;
using Domain;

public class CustomerOrderProcessor(RefactoringChallengeDbContext dbContext, IDiscountResolver discountResolver)
{
    /// <summary>
    /// Process all new orders for specific customer. Update discount and status.
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>List of processed orders</returns>
    public async Task<List<Order>> ProcessCustomerOrdersAsync(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentException("ID zákazníka musí být kladné číslo.", nameof(customerId));

        var processedOrders = new List<Order>();
        
        var customer = await dbContext.Customers.SingleOrDefaultAsync(c => c.Id == customerId);
            
        if (customer == null)
            throw new Exception($"Zákazník s ID {customerId} nebyl nalezen.");

        var pendingOrders = await dbContext.Orders
            .Where(o => o.CustomerId == customerId && o.Status == "Pending")
            .Include(order => order.Items)
            .ThenInclude(orderItem => orderItem.Product)
            .ToListAsync();

        foreach (var order in pendingOrders)
        {
            var totalAmount = order.Items.Sum(orderItem => orderItem.Quantity * orderItem.UnitPrice);
            var discountPercent = discountResolver.GetDiscount(customer, totalAmount);

            var discountAmount = totalAmount * (discountPercent / 100);
            var finalAmount = totalAmount - discountAmount;
                
            order.DiscountPercent = discountPercent;
            order.DiscountAmount = discountAmount;
            order.TotalAmount = finalAmount;
            order.Status = "Processed";
            
            await dbContext.SaveChangesAsync();

            var outOfStockItemsCount = await dbContext.Database
                .SqlQueryRaw<int?>(
                    "SELECT COUNT(*) AS Value FROM OrderItems AS oi LEFT JOIN Inventory AS i ON oi.ProductId = i.ProductId WHERE OrderId = {0} AND i.StockQuantity < oi.Quantity",
                    new SqlParameter("OrderId", order.Id))
                .SingleOrDefaultAsync();
            
            var allProductsAvailable = outOfStockItemsCount == 0;

            string orderLogsMessage;
            if (allProductsAvailable)
            {
                foreach (var item in order.Items)
                {
                    await dbContext.Database.ExecuteSqlRawAsync(
                        "UPDATE Inventory SET StockQuantity = StockQuantity - {0} WHERE ProductId = {1}",
                        new SqlParameter("Quantity", item.Quantity), new SqlParameter("ProductId", item.ProductId));
                }
                    
                order.Status = "Ready";
                orderLogsMessage = $"Order completed with {order.DiscountPercent}% discount. Total price: {order.TotalAmount}";
            }
            else
            {
                order.Status = "OnHold";
                orderLogsMessage = "Order on hold. Some items are not on stock.";
            }
            
            await dbContext.SaveChangesAsync();
                
            await dbContext.Database.ExecuteSqlRawAsync(
                "INSERT INTO OrderLogs (OrderId, LogDate, Message) VALUES ({0}, {1}, {2})",
                new SqlParameter("Quantity", order.Id), 
                new SqlParameter("ProductId", DateTime.Now),
                new SqlParameter("Message", orderLogsMessage));
                
            processedOrders.Add(order);
        }
        
        return processedOrders;
    }
}
