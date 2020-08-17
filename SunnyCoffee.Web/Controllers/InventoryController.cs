using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SunnyCoffee.Services.Inventory;
using SunnyCoffee.Web.Serialization;
using SunnyCoffee.Web.ViewModels;

namespace SunnyCoffee.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger _logger;

        public InventoryController(IInventoryService inventoryService, ILogger logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory()
        {
            _logger.LogInformation("Getting an inventory");
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(pi => new ProductInventoryModel
                {
                    Id = pi.Id,
                    Product = ProductMapper.SerializeProductModel(pi.Product),
                    IdealQuantity = pi.IdealQuantity,
                    QuantityOnHand = pi.QuantityOnHand
                })
                .OrderBy(inv => inv.Product.Name)
                .ToList();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            _logger.LogInformation("Updating an inventory");

            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            var inventory = _inventoryService.UpdateUnitsAvaliable(id, adjustment);
            
            return Ok(inventory);
        }

    }
}
