using System;
using System.Collections.Generic;
using System.Linq;
using SunnyCoffee.Data;
using SunnyCoffee.Data.Models;

namespace SunnyCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SunnyDbContext _db;
        public ProductService(SunnyDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _db.Products.Find(id);
                product.IsArchived = true;
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Archived product",
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = "Exception when Archived product",
                    IsSuccess = false
                };
            }

           
        }

        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);
                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };
                _db.ProductInventories.Add(newInventory);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.Now,
                    IsSuccess = false,
                    Message = "Error saving new product"
                };
            }

            return new ServiceResponse<Data.Models.Product>
            {
                Data = product,
                Time = DateTime.Now,
                Message = "Saved new Product",
                IsSuccess = true
            };
          
        }

        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        public Data.Models.Product GetProductById(int id)
        {
            return _db.Products.Find(id);
        }
    }
}
