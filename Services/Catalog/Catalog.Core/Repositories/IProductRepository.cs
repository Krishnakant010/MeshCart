using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams specParams);
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName);
    Task<Product> GetProductByIdAsync(string productId);
    Task<Product> CreatProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(string id);
    Task<ProductBrand> GetBrandByIdAsync(string id);
    Task<ProductType> GetTypeByIdAsync(string id);
}