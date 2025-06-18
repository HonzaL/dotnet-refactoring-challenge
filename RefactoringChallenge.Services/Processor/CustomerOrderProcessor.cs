using RefactoringChallenge.Domain;
using RefactoringChallenge.Services.Abstractions;

namespace RefactoringChallenge.Services.Processor;

public class CustomerOrderProcessor(
    IDiscountService discountService,
    ICustomerService customerService,
    IOrderService orderService, 
    IInventoryService inventoryService,
    IOrderLogService orderLogService)
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
        
        var customer = await customerService.FindByIdAsync(customerId);
            
        if (customer == null)
            throw new Exception($"Zákazník s ID {customerId} nebyl nalezen.");

        var pendingOrders = await orderService.GetCustomerPendingOrdersAsync(customerId);

        foreach (var order in pendingOrders)
        {
            var totalAmount = order.Items.Sum(orderItem => orderItem.Quantity * orderItem.UnitPrice);
            var discountPercent = discountService.GetDiscount(customer, totalAmount);

            var discountAmount = totalAmount * (discountPercent / 100);
            var finalAmount = totalAmount - discountAmount;
                
            order.DiscountPercent = discountPercent;
            order.DiscountAmount = discountAmount;
            order.TotalAmount = finalAmount;
            order.Status = "Processed";
            
            await orderService.SaveAsync(order);
            
            var allProductsAvailable = await inventoryService.IsAllOrderItemsInStockAsync(order.Id);

            string orderLogsMessage;
            if (allProductsAvailable)
            {
                await inventoryService.RemoveOrderItemsFromStockAsync(order.Id);
                order.Status = "Ready";
                orderLogsMessage = $"Order completed with {order.DiscountPercent}% discount. Total price: {order.TotalAmount}";
            }
            else
            {
                order.Status = "OnHold";
                orderLogsMessage = "Order on hold. Some items are not on stock.";
            }
            
            await orderService.SaveAsync(order);
            
            await orderLogService.AddAsync(order.Id, orderLogsMessage);

            processedOrders.Add(order);
        }
        
        return processedOrders;
    }
}
