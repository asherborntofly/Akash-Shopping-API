using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Businness;
using ShopBridge.DTO;

namespace ShopBridge.API
{
    [ApiController]
    [Route("product")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductService productService, ILogger<ProductController> logger)
        {
            _service = productService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _service.GetProduct(id);
                if (product == null)
                    return NotFound();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting product with id {id}");
                return StatusCode(500, "An error occurred while retrieving the product");
            }
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult<List<Product>>> GetProducts(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var products = await _service.GetAllProducts(page, pageSize, string.IsNullOrEmpty(searchTerm) ? string.Empty : searchTerm);
            return new JsonResult(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest();

                var addedProduct = await _service.AddProduct(product);

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding product {product.Name}");
                return StatusCode(500, "An error occurred while adding the product");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            try
            {
                var updatedProduct = await _service.UpdateProduct(id, product);
                return updatedProduct;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            try
            {
                await _service.DeleteProduct(id);
                return new JsonResult("SuccessFully Ddeleted");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}