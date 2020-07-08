using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SunnyCoffee.Services.Product;
using SunnyCoffee.Web.Serialization;

namespace SunnyCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");

            var products = _productService.GetAllProducts();
            var productsViewModel = products.Select(p => ProductMapper.SerializeProductModel(p));

            return Ok(productsViewModel);
        }
    }
}
