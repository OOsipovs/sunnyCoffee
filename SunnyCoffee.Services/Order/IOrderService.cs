using System;
using System.Collections.Generic;
using SunnyCoffee.Data.Models;

namespace SunnyCoffee.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order);
        ServiceResponse<bool> MarkFulfilled(int id);
    }
}
