using Microsoft.EntityFrameworkCore;

namespace RefactoringChallenge.Dal.Processor;

using Microsoft.Data.SqlClient;
using Domain;

public class CustomerOrderProcessor(RefactoringChallengeDbContext dbContext)
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
            decimal totalAmount = 0;
            foreach (var item in order.Items)
            {
                var subtotal = item.Quantity * item.UnitPrice;
                totalAmount += subtotal;
            }

            decimal discountPercent = 0;

            if (customer.IsVip)
            {
                discountPercent += 10;
            }

            int yearsAsCustomer = DateTime.Now.Year - customer.RegistrationDate.Year;
            if (yearsAsCustomer >= 5)
            {
                discountPercent += 5;
            }
            else if (yearsAsCustomer >= 2)
            {
                discountPercent += 2;
            }

            if (totalAmount > 10000)
            {
                discountPercent += 15;
            }
            else if (totalAmount > 5000)
            {
                discountPercent += 10;
            }
            else if (totalAmount > 1000)
            {
                discountPercent += 5;
            }

            if (discountPercent > 25)
            {
                discountPercent = 25;
            }

            var discountAmount = totalAmount * (discountPercent / 100);
            var finalAmount = totalAmount - discountAmount;
                
            order.DiscountPercent = discountPercent;
            order.DiscountAmount = discountAmount;
            order.TotalAmount = finalAmount;
            order.Status = "Processed";
            
            await dbContext.SaveChangesAsync();
            
            
            var allProductsAvailable = true;
            foreach (var item in order.Items)
            {
                var inventory = await dbContext.Database
                    .SqlQueryRaw<Inventory?>("SELECT * FROM Inventory WHERE ProductId = {0}",
                        new SqlParameter("ProductId", item.ProductId)).SingleOrDefaultAsync();
                if (inventory?.StockQuantity != null && !(inventory.StockQuantity < item.Quantity))
                {
                    continue;
                }
                
                allProductsAvailable = false;
                break;
            }

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
