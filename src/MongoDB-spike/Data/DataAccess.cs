using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Data.Model;
using MongoDB.Bson;

namespace Data
{
    public class DataAccess
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        private static IMongoCollection<Waypoint> _collection;

        private static readonly IList<Waypoint> _waypoints = new List<Waypoint>()
        {
            new Waypoint(-32.759965, 151.577419, 20.0),
            new Waypoint(-32.760234, 151.577629, 20.0),
            new Waypoint(-32.760309, 151.577513, 20.0),
            new Waypoint(-32.760058, 151.577258, 20.0)
        };

        private static void Initialise()
        {
            _client = new MongoClient("mongodb://localhost");
            _database = _client.GetDatabase("Data");
        }

        public static void ReadAndWriteThings()
        {
            Initialise();

            EnsureCollectionExists();

            DeleteThings();
            InsertThings();
            UpdateThings();
            ReadThings();
        }

        private static void UpdateThings()
        {
            var filter = Builders<Waypoint>.Filter.Eq("Latitude", -32.759965);
            var update = Builders<Waypoint>.Update.Set(w => w.AltitudeMetres, 40);
            UpdateResult updateResult = _collection.UpdateOne(filter, update);

            Console.WriteLine("Updated waypoint to double altitude");
        }

        private static void DeleteThings()
        {
            _collection.DeleteMany(new BsonDocument());
            Console.WriteLine("Deleted all waypoints");
        }

        private static void ReadThings()
        {
            Waypoint waypoint = _collection.AsQueryable().Where(w => w.Latitude == -32.759965).First();
            Console.WriteLine("Queried waypoint with ID " + waypoint.Id);

            List<Waypoint> waypoints = _collection.AsQueryable().Where(w => w.Latitude == -32.759965 || w.Latitude == -32.760058).ToList();
            Console.WriteLine("Queried " + waypoints.Count + " other waypoints");
        }

        private static void InsertThings()
        {
            _collection.InsertMany(_waypoints);
            Console.WriteLine("Inserted " + _waypoints.Count + " waypoints");
        }

        private static void EnsureCollectionExists()
        {
            _collection = _database.GetCollection<Waypoint>("Waypoints");
            if (_collection == null)
                _database.CreateCollection("Waypoints");
        }
    }
}
