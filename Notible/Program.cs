using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

using Microsoft.Extensions.Configuration;

using Notible;

class Program
{
    private const byte ADD_ONE = 1;
    private const byte UPDATE = 2;
    private const byte SEARCH = 3;
    private const byte DELETE = 4;
    private const byte EXIT = 5;
    
    private const int SEARCH_PROPERTY_NAME = 1;
    private const int SEARCH_PROPERTY_LOCATION = 2;
    private const int SEARCH_PROPERTY_PRICE = 3;
    private const int SEARCH_PROPERTY_IS_HEALTHY = 4;
    private const int SEARCH_PROPERTY_IS_GOOD_PRICE = 5;
    private const int SEARCH_PROPERTY_ID = 6;
    
    private static IMongoDatabase myDb;
    private static IMongoCollection<BsonDocument> collection;
    
    static void Main()
    {
        InitMongoConnection();
        bool exitProgram = false;
        
        int choice;
        bool validChoice;
        
        
        while (!exitProgram)
        {
            Console.WriteLine("Notible");
            Console.WriteLine("Enter and option:\n1) Add a new food entry\n2) Update a food entry\n3) Search existing food entries\n4) Delete a food entry\n5) Exit");
            
            choice = Helper.GetValidIntInput(ADD_ONE, EXIT);
        
            switch (choice)
            {
                case ADD_ONE:
                    AddOneFoodEntry();
                    break;
            
                case UPDATE:
                    UpdateFoodEntry();
                    break;
            
                case SEARCH:
                    SearchFoodEntry();
                    break;
            
                case DELETE:
                    DeleteEntry();
                    break;
                
                case EXIT:
                    exitProgram = true;
                    break;
            }
        }
    }

    private static void AddOneFoodEntry()
    {
        Console.WriteLine("Enter food name: ");
        string foodName = Console.ReadLine().ToLower();
        
        Console.WriteLine("Enter food price: ");
        int price = Helper.GetValidIntInput(0, Int32.MaxValue);
        
        Console.WriteLine("Enter location where you can purchase this food: ");
        string location = Console.ReadLine().ToLower();
        
        Console.WriteLine("Is this food Healthy?(y/n): ");
        bool isHealthy = Helper.GetValidBoolInput();
        
        Console.WriteLine("Is this food at a good price?(y/n): ");
        bool isGoodPrice = Helper.GetValidBoolInput();
        
        var entry = new MealEntry
        {
            FoodName = foodName,
            Price = price,
            Location = location,
            IsHealthy = isHealthy,
            IsGoodPrice = isGoodPrice
        };
        
        collection.InsertOne(entry.ToBsonDocument());
    }

    private static void UpdateFoodEntry()
    {
        Console.WriteLine("Enter the exact ID of the food entry you would like to update:");
        string enteredFoodId = Helper.GetValidHexInput();
        ObjectId documentId = new ObjectId(enteredFoodId);
        
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(MealEntry.ID_STR, documentId);
        UpdateDefinition<BsonDocument> update = null;
        List<BsonDocument> results = collection.Find(filter).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No food entry found to update.");
            return;
        }
        
        Console.WriteLine("Enter the property you would like to edit:\nName (1)\nLocation (2)\nPrice (3)\nIs Healthy (4)\nIs Good Price (5)");
        int choice = Helper.GetValidIntInput(1, 5);
        
         switch (choice)
        {
            case SEARCH_PROPERTY_NAME:
                Console.WriteLine("Re-enter food name: ");
                string foodName = Console.ReadLine().ToLower();
                
                update = Builders<BsonDocument>.Update.Set(MealEntry.FOOD_NAME_STR, foodName);
                break;
            
            case SEARCH_PROPERTY_LOCATION:
                Console.WriteLine("Re-enter food location: ");
                string location = Console.ReadLine().ToLower();

                update = Builders<BsonDocument>.Update.Set(MealEntry.LOCATION_STR, location);
                break;
            
            case SEARCH_PROPERTY_PRICE:
                Console.WriteLine("Re-enter food price: ");
                int price = Helper.GetValidIntInput(0, Int32.MaxValue);
                
                update = Builders<BsonDocument>.Update.Set(MealEntry.PRICE_STR, price);
                break;
            
            case SEARCH_PROPERTY_IS_HEALTHY:
                Console.WriteLine("Is this food Healthy?(y/n): ");
                bool isHealthy = Helper.GetValidBoolInput();
                
                update = Builders<BsonDocument>.Update.Set(MealEntry.IS_HEALTHY_STR, isHealthy);
                break;
            
            case SEARCH_PROPERTY_IS_GOOD_PRICE:
                Console.WriteLine("Is this food at a good price?(y/n): ");
                bool isGoodPrice = Helper.GetValidBoolInput();
                
                update = Builders<BsonDocument>.Update.Set(MealEntry.IS_GOOD_PRICE_STR, isGoodPrice);
                break;
            
            default:
                throw new ArgumentOutOfRangeException("Invalid choice");
        }
        
        collection.UpdateOne(filter, update);
        Console.WriteLine("Successfully updated food entry");
    }
    
    private static void SearchFoodEntry()
    {
        Console.WriteLine("Enter the property you would like to search:\nName (1)\nLocation (2)\nPrice (3)\nIs Healthy (4)\nIs Good Price (5)\nID (6)");
        int choice = Helper.GetValidIntInput(1, 6);
        FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
        List<BsonDocument> results = new List<BsonDocument>();
        
        switch (choice)
        {
            case SEARCH_PROPERTY_NAME:
                Console.WriteLine("Enter the name of the food you would like to search:");
                string foodName = Console.ReadLine().ToLower();
            
                filter = Builders<BsonDocument>.Filter.Eq(MealEntry.FOOD_NAME_STR, foodName);
                results = collection.Find(filter).ToList();
                break;
            
            case SEARCH_PROPERTY_LOCATION:
                Console.WriteLine("Enter the location you would like to search:");
                string foodLoc = Console.ReadLine().ToLower();
            
                filter = Builders<BsonDocument>.Filter.Eq(MealEntry.LOCATION_STR, foodLoc);
                results = collection.Find(filter).ToList();
                break;
            
            case SEARCH_PROPERTY_PRICE:
                Console.WriteLine("Enter the lowest price of the foods you would like to search:");
                int lowerBound = Helper.GetValidIntInput(0, Int32.MaxValue);
                
                Console.WriteLine("Enter the highest price of the foods you would like to search:");
                int upperBound = Helper.GetValidIntInput(lowerBound, Int32.MaxValue);

                filter = Builders<BsonDocument>.Filter.Gte(MealEntry.PRICE_STR, lowerBound);
                filter &= Builders<BsonDocument>.Filter.Lte(MealEntry.PRICE_STR, upperBound);
                results = collection.Find(filter).ToList();
                break;
            
            case SEARCH_PROPERTY_IS_HEALTHY:
                Console.WriteLine("Search for healthy food (y) or not healthy food (n)?(y/n): ");
                bool enteredHealthy = Helper.GetValidBoolInput();
            
                filter = Builders<BsonDocument>.Filter.Eq(MealEntry.IS_HEALTHY_STR, enteredHealthy);
                results = collection.Find(filter).ToList();
                break;
            
            case SEARCH_PROPERTY_IS_GOOD_PRICE:
                Console.WriteLine("Search for well priced food (y) or not well priced food (n)?(y/n): ");
                bool enteredPrice = Helper.GetValidBoolInput();
            
                filter = Builders<BsonDocument>.Filter.Eq(MealEntry.IS_GOOD_PRICE_STR, enteredPrice);
                results = collection.Find(filter).ToList();
                break;
            
            case SEARCH_PROPERTY_ID:
                Console.WriteLine("Enter the exact ID of the food entry you would like to search:");
                string enteredFoodId = Helper.GetValidHexInput();
                ObjectId documentId = new ObjectId(enteredFoodId);
                
                filter = Builders<BsonDocument>.Filter.Eq(MealEntry.ID_STR, documentId);
                results = collection.Find(filter).ToList();
                break;
            
            default:
                throw new ArgumentOutOfRangeException("Invalid choice");
        }

        if (results.Count > 0)
        {
            MealEntry entry = null;
            foreach (var r in results)
            {
                entry = BsonSerializer.Deserialize<MealEntry>(r);
                Console.WriteLine("----------\n" + entry.FormattedString());
            }
        
            Console.WriteLine("----------");
        }
        else
        {
            Console.WriteLine("No food entries found");
        }
    }
    
    private static void DeleteEntry()
    {
        Console.WriteLine("Enter the 24 character hexadecimal id of the food entry you would like to delete:");
        string enteredFoodId = Helper.GetValidHexInput();
        ObjectId documentId = new ObjectId(enteredFoodId);
        
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(MealEntry.ID_STR, documentId);
        DeleteResult res = collection.DeleteOne(filter);
        
        if (res.DeletedCount > 0)
        {
            Console.WriteLine("Entry deleted successfully.");
        }
        else
        {
            Console.WriteLine($"No food entries found with and ID of {enteredFoodId}.");
        }
    }
    
    /// <summary>
    /// Loads environment variables into the program
    /// </summary>
    /// <param name="filePath">The path to the environment file</param>
    private static void LoadEnvironmentFile(string filePath)
    {
        //Exit if there is no file
        if (!File.Exists(filePath))
            return;

        //Load each line in the file
        foreach (var line in File.ReadAllLines(filePath))
        {
            //Split the line between the equals sign
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);
            
            //Skip the line if it is not split into name and value
            if (parts.Length != 2)
                continue;
            
            //Set the new environment variable
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }

    private static void InitMongoConnection()
    {
        //Creates a class map for the MealEntry class so it can be treated as a BSON doucment
        BsonClassMap.RegisterClassMap<MealEntry>(classMap =>
        {
            classMap.MapIdMember(e => e.Id);
            classMap.MapMember(e => e.FoodName);
            classMap.MapMember(e => e.Price);
            classMap.MapMember(e => e.Location);
            classMap.MapMember(e => e.IsHealthy);
            classMap.MapMember(e => e.IsGoodPrice);
        });
        
        //Load and initialize the environment file with the MongoDB credentials
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        LoadEnvironmentFile(dotenv);
        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        
        //Create the connection string
        string mongodbUri =
            $"mongodb+srv://{config["USERNAME"]}:{config["PASSWORD"]}@cluster0.ax7ze.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        
        //Initialize client settings
        var settings = MongoClientSettings.FromConnectionString(mongodbUri); 
    
        // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    
        // Create a new client and connect to the server
        var client = new MongoClient(settings);
        
        // Send a ping to confirm a successful connection
        try 
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex);
        }
        
        //Gets the database and collection from the client
        myDb = client.GetDatabase(config["DATABASENAME"]);
        collection = myDb.GetCollection<BsonDocument>(config["COLLECTIONNAME"]);
    }
}