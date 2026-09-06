using System.Text.Json;
using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public static class DatabaseSeeder
{
    // public DatabaseSeeder(IOptions<DataBaseSettings>options)
    // {
    //     var settings = options.Value;
    //     var client = new MongoClient(settings.ConnectionString);
    //     var db = client.GetDatabase(settings.DatabaseName);
    //     _brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
    //     _types = db.GetCollection<ProductType>(settings.TypeCollectionName);
    //     _products = db.GetCollection<Product>(settings.BrandCollectionName);
    // }
    public static async Task SeedAsync(IOptions<DataBaseSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var db = client.GetDatabase(settings.DatabaseName);
        var brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
        var types = db.GetCollection<ProductType>(settings.TypeCollectionName);
        var products = db.GetCollection<Product>(
            settings.ProductCollectionName);

        var seedBasePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "SeedData");
        // seed brands
        List<ProductBrand>? brandList = new();
        List<ProductType>? typeList = new();
        List<Product>? productList = new();
        if ((await brands.CountDocumentsAsync(_ => true) == 0))
        {
            var brandData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "brands.json"));
            brandList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            await brands.InsertManyAsync(brandList);
        }
        else
        {
            brandList = await brands.Find(_ => true).ToListAsync();
        }

        if ((await types.CountDocumentsAsync(_ => true) == 0))
        {
            var productTypeData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "types.json"));
            typeList = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
            await types.InsertManyAsync(typeList);
        }
        else
        {
            typeList = await types.Find(_ => true).ToListAsync();
        }

        if ((await products.CountDocumentsAsync(_ => true) == 0))
        {
            var productTypeData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "products.json"));
            productList = JsonSerializer.Deserialize<List<Product>>(productTypeData);
            foreach (var product in productList)
            {
                //reset id to let mongo db generate 
                product.Id = null;
                if (product.CreatedDate == default) product.CreatedDate = DateTimeOffset.UtcNow;
            }
            await products.InsertManyAsync(productList);
        }
        else
        {
            productList = await products.Find(_ => true).ToListAsync();
        }
    }
}