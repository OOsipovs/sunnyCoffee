using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SunnyCoffee.Data;
using SunnyCoffee.Data.Models;
using SunnyCoffee.Services.Inventory;
using SunnyCoffee.Services.Product;

namespace SunnyCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private SunnyDbContext _db;
        private IInventoryService _inventorySerice;
        private IProductService _productService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(SunnyDbContext dbContext, IInventoryService inventoryService, IProductService productService, ILogger<OrderService> logger)
        {
            _db = dbContext;
            _logger = logger;
            _inventorySerice = inventoryService;
            _productService = productService;
        }

        public ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order)
        {
            _logger.LogInformation("Generating new order");

            foreach(var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);

                var inventoryId = _inventorySerice.GetByProductId(item.Product.Id).Id;

                _inventorySerice.UpdateUnitsAvaliable(inventoryId, -item.Quantity);
            }

            try
            {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Order is not created due to exception"
                };
            }

            return new ServiceResponse<bool>
            {
                IsSuccess = true,
                Data = true,
                Message = "Order Created"
            };
        }

        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
                .Include(so => so.Customer)
                    .ThenInclude(c => c.PrimaryAddress)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(it => it.Product)
                .ToList();
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var order = _db.SalesOrders.Find(id);
            order.UpdatedOn = DateTime.UtcNow;
            order.IsPaid = true;

            try
            {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false
                };
            }

        }
    }
}
