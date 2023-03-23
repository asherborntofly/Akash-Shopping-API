using Microsoft.Extensions.Caching.Memory;
using ShopBridge.DataAccess;
using ShopBridge.DTO;

namespace ShopBridge.Businness
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

        public ProductService(ProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
            _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        }

        public async Task<Product> AddProduct(Product product)
        {
            //Validate Input
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var addedProduct = await _productRepository.AddProductAsync(product);
            return addedProduct;
        }

        public async Task<Product> UpdateProduct(int id, Product product)
        {
            //Validate Input
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            //Add any necessary data validations here
            var existingProduct = await _productRepository.GetProductAsync(id);
            
            if (existingProduct is null)
                throw new ProductException($"Product with ID {id} not found.");


            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            Product updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);

            //Updating the Cache
            _cache.Remove($"product-{id}");
            _cache.Set($"product-{id}", updatedProduct, _memoryCacheEntryOptions);

            return updatedProduct;
        }

        public async Task DeleteProduct(int id)
        {
            bool productExist = _productRepository.HasProductAsync(id);
            if(!productExist)
                throw new ProductException($"Product with ID {id} not found.");

            await _productRepository.DeleteProductAsync(id);
            _cache.Remove($"product-{id}");
        }

        public async Task<List<Product>> GetAllProducts(int page, int pageSize, string searchTerm)
        {
            var products = await _cache.GetOrCreateAsync($"products-all-search-{page}-{pageSize}-{searchTerm}", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(5);
                return await _productRepository.GetAllProductsAsync(page, pageSize, searchTerm);
            });

            if (products is null)
                throw new ProductException($"Not able to get all Products");

            return products;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _cache.GetOrCreateAsync($"product-{id}", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(5);
                return await _productRepository.GetProductAsync(id);
            });

            if(product is null)
                throw new ProductException($"Product with ID {id} not found.");

            return product;
        }
    }
}
