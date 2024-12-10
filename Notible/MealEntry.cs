using MongoDB.Bson.Serialization.Attributes;

namespace Notible;

internal class MealEntry
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]     
    public string Id { get; set; }
     
    [BsonElement("food_name")]
    public string FoodName { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }
    
    [BsonElement("location")]
    public string Location { get; set; }

    [BsonElement("is_healthy")]
    public bool IsHealthy { get; set; } 
    
    [BsonElement("is_good_price")]
    public bool IsGoodPrice { get; set; } 
}