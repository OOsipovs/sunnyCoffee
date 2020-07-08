using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SunnyCoffee.Data;
using SunnyCoffee.Data.Models;

namespace SunnyCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private SunnyDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SunnyDbContext sunnyDbContext, ILogger<InventoryService> logger)
        {
            _db = sunnyDbContext;
            _logger = logger;
        }

        private void CreateSnapshot(ProductInventory inventory)
        {
            var now = DateTime.UtcNow;
            var snapshot = new ProductInventorySnapshot
            {
                SnapshotTime = now,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };

            _db.Add(snapshot);
        }
            
        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories.Include(pi => pi.Product).FirstOrDefault(pi => pi.Id == productId);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _db.ProductInventorySnapshots
                .Include(ps => ps.Product)
                .Where(ps => ps.SnapshotTime > earliest && !ps.Product.IsArchived)
                .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvaliable(int id, int adjustment)
        {
            try
            {
                var inventory = _db.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Id == id);

                inventory.QuantityOnHand += adjustment;
                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error creating inventory snapshot");
                }

                _db.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = inventory,
                    Message = "Product inventory adjusted",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ex.InnerException.ToString(),
                    Time = DateTime.UtcNow
                };

            }
        }
    }
}
