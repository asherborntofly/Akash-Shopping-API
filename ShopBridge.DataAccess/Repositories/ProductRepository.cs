using Microsoft.EntityFrameworkCore;
using ShopBridge.DTO;
using System.Linq;

namespace ShopBridge.DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopBridgeDbContext _dbContext;

        public ProductRepository(ShopBridgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllProductsAsync(int page, int pageSize, string searchTerm)
        {
            var productsQuery = _dbContext.Products.AsQueryable();

            // Filter by search query
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            // Calculate skip and take values for pagination
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            // Apply pagination
            var products = await productsQuery
                .OrderBy(p => p.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return products;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _dbContext.Set<Product>().Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            //return await _dbContext.Set<Product>().FindAsync(id);
            return await _dbContext.Set<Product>().FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await GetProductAsync(productId);
            if (product != null)
            {
                _dbContext.Set<Product>().Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public bool HasProductAsync(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
        }
    }
}
