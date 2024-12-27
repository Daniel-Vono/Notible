using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Notible;

internal sealed class MealEntry
{
    public const string ID_STR = "_id";
    public const string FOOD_NAME_STR = "FoodName";
    public const string PRICE_STR = "Price";
    public const string LOCATION_STR = "Location";
    public const string IS_HEALTHY_STR = "IsHealthy";
    public const string IS_GOOD_PRICE_STR = "IsGoodPrice";
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]     
    public ObjectId Id { get; set; }
     
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
    
    public string FormattedString()
    {
        return $"Entry ID: {Id.ToString()}\nFood Name {FoodName}\nPrice: ${Price}\nLocation: {Location}\nHealthy? {IsHealthy}\nGood Price? {IsGoodPrice}";
    }
}