using System;
using System.IO;
using Microsoft.Extensions.Configuration;

using MongoDB.Driver;
using MongoDB.Bson;


class Program
{
    static void Main()
    {
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
}