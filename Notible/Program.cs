using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

using Notible;

class Program
{
    private const byte ADD_ONE = 1;
    private const byte UPDATE = 2;
    private const byte SEARCH = 3;
    private const byte DELETE = 4;
    private const byte EXIT = 5;
    
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
                    break;
            
                case SEARCH:
                    break;
            
                case DELETE:
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
        string foodName = Console.ReadLine();
        
        Console.WriteLine("Enter food price: ");
        int price = Helper.GetValidIntInput(0, Int32.MaxValue);
        
        Console.WriteLine("Enter location where you can purchase this food: ");
        string location = Console.ReadLine();
        
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
            classMap.MapMember(e => e.Id);
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