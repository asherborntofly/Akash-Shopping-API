using ShopBridge.DTO;

namespace ShopBridge.DataAccess
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<Product> GetProductAsync(int id);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<List<Product>> GetAllProductsAsync(int page, int pageSize, string searchTerm);
        bool HasProductAsync(int id);
    }
}
