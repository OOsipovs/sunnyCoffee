using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SunnyCoffee.Services.Customer;
using SunnyCoffee.Services.Order;
using SunnyCoffee.Web.Serialization;
using SunnyCoffee.Web.ViewModels;

namespace SunnyCoffee.Web.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, ICustomerService customerService)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody]InvoiceModel invoice)
        {
            _logger.LogInformation("Generating new order");
            var order = OrderMapper.SerializeInvoiceToOrder(invoice);
            order.Customer = _customerService.GetById(invoice.CustomerId);
            _orderService.GenerateInvoiceForOrder(order);

            return Ok();
        }

        [HttpPost("/api/order")]
        public ActionResult GetOrders()
        {
            var orders = _orderService.GetOrders();
            var orderModels = OrderMapper.SerializeOrdersToViewModels(orders);
            return Ok(orderModels);
        }

        [HttpPatch("/api/order/complete/{id}")]
        public ActionResult GetOrders(int id)
        {
            _logger.LogInformation($"Making order {id} complete...");
            _orderService.MarkFulfilled(id);
            return Ok();
        }
    }
}
