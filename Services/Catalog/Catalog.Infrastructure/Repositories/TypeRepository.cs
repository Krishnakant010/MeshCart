using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class TypeRepository:ITypeRepository
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<ProductType> _types;
    public TypeRepository(IOptions<DataBaseSettings>options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        _types = db.GetCollection<ProductType>(settings.TypeCollectionName);
    }
    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        // var cursor =  _types.Find(_ => true);
        // var types = await cursor.ToListAsync();
        // return types;
        return await _types.Find(_ => true).ToListAsync();
    }

    public async Task<ProductType> GetByIdAsync(string id)
    {
        return await _types.Find(t => t.Id.Equals(id)).FirstOrDefaultAsync();
    }
}