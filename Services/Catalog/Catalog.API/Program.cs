using System.Reflection;
using Catalog.Application.Handlers;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;using Microsoft.Extensions.Options;
using Catalog.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. to see t readable else its binary
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddEndpointsApiExplorer();
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(), typeof(GetAllBrandsHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddSwaggerGen();
builder.Services.Configure<DataBaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<DataBaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var options = scope.ServiceProvider
        .GetRequiredService<IOptions<DataBaseSettings>>();

    await DatabaseSeeder.SeedAsync(options);
    
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();