using System;
using System.Collections.Generic;
using SunnyCoffee.Data.Models;

namespace SunnyCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        public OrderService()
        {
        }

        public ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order)
        {
            throw new NotImplementedException();
        }

        public List<SalesOrder> GetOrders()
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            throw new NotImplementedException();
        }
    }
}
