using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace Catalog.Core.Entities;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
}