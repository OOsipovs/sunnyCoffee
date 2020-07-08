using System;
using System.Collections.Generic;
using SunnyCoffee.Data.Models;

namespace SunnyCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        List<ProductInventory> GetCurrentInventory(); 
        ServiceResponse<ProductInventory> UpdateUnitsAvaliable(int id, int adjustment);
        ProductInventory GetByProductId(int productId);
        List<ProductInventorySnapshot> GetSnapshotHistory();
    }
}
