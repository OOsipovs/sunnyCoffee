using System;
using System.Collections.Generic;
using System.Linq;
using SunnyCoffee.Data.Models;
using SunnyCoffee.Web.ViewModels;

namespace SunnyCoffee.Web.Serialization
{
    public static class OrderMapper
    {
        public static SalesOrder SerializeInvoiceToOrder(InvoiceModel invoice)
        {
            var salesOrderItems = invoice.LineItems.Select(
                    item => new SalesOrderItem
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        Product = ProductMapper.SerializeProductModel(item.Product)
                    }
                );

            return new SalesOrder
            {
                SalesOrderItems = salesOrderItems.ToList(),
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
        }

        public static List<OrderModel> SerializeOrdersToViewModels(IEnumerable<SalesOrder> orders)
        {
            return orders.Select(order => new OrderModel {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                UpdatedOn = order.UpdatedOn,
                SalesOrderItems = SerializeSalesOrderItems(order.SalesOrderItems),
                Customer = CustomerMapper.SerializeCustomer(order.Customer),
                IsPaid = order.IsPaid
            }).ToList();
        }

        private static List<SalesOrderItemModel> SerializeSalesOrderItems(IEnumerable<SalesOrderItem> orderItems)
        {
            return orderItems.Select(order => new SalesOrderItemModel
            {
                Id = order.Id,
                Quantity = order.Quantity,
                Product = ProductMapper.SerializeProductModel(order.Product)
            }).ToList();
        }
    }
}
