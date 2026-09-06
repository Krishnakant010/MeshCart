using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Extensions;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository:IProductRepository
{
    private readonly IMongoCollection<Product> _products;
    private readonly IMongoCollection<ProductType> _types;
    private readonly IMongoCollection<ProductBrand> _brands;
    
     public ProductRepository(IOptions<DataBaseSettings>options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
        _types = db.GetCollection<ProductType>(settings.TypeCollectionName);
        _products = db.GetCollection<Product>(settings.ProductCollectionName);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _products.Find(_ => true).ToListAsync();
    }

    public async Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams specParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrEmpty(specParams.Search))
        {
            filter &= builder.Where(p => p.Name.ToLower().Contains(specParams.Search.ToLower()));
        }
        if (!string.IsNullOrEmpty(specParams.BrandId))
        {
            filter &= builder.Eq(p => p.Brand.Id, specParams.BrandId);
        }
        
        if (!string.IsNullOrEmpty(specParams.TypeId))
        {
            filter &= builder.Eq(p => p.Type.Id, specParams.TypeId);
        }

        var totalItems = await _products.CountDocumentsAsync(filter);
        var data = await ApplyDataFilters(specParams, filter);
        return new Pagination<Product>(
            specParams.PageIndex, specParams.PageSize, (int)totalItems, data
        );
    }


    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
       var filter =Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression($".*{name}.*", "i"));
       return await _products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brandName)
    {
        return await _products.Find(b => b.Brand.Name.EqualsCaseInsensitive(brandName)).ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string productId)
    {
        return await _products.Find(p => p.Id.Equals(productId)).FirstOrDefaultAsync();
    }

    public async Task<Product> CreatProductAsync(Product product)
    {
        await _products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var updatedProduct =await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var deletedProduct = await _products.DeleteOneAsync(p => p.Id.Equals(id));
        return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
    }

    public async Task<ProductBrand> GetBrandByIdAsync(string id)
    {
        return await _brands.Find(b => b.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<ProductType> GetTypeByIdAsync(string id)
    {
        return await _types.Find(t => t.Id.Equals(id)).FirstOrDefaultAsync();
    }
    private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams specParams, FilterDefinition<Product> filter)
    {
        var sortDefn = Builders<Product>.Sort.Ascending("Name");
        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            sortDefn = specParams.Sort switch
            {
                "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                _ => Builders<Product>.Sort.Ascending(p => p.Name)
            };
        }

        return await _products.Find(filter).Sort(sort: sortDefn).Skip(specParams.PageSize * (specParams.PageIndex - 1))
            .Limit(specParams.PageSize).ToListAsync();
    }

}