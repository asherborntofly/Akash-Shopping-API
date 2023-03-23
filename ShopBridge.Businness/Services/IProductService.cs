using ShopBridge.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Businness
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product product);
        Task<Product> GetProduct(int id);
        Task<Product> UpdateProduct(int id, Product product);
        Task DeleteProduct(int id);
        Task<List<Product>> GetAllProducts(int page, int pageSize, string searchTerm);
    }
}
