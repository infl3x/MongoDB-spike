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
        private static IMongoCollection<Waypoint> _waypointCollection;
        private static IMongoCollection<WaypointType> _waypointTypeCollection;

        private static IList<WaypointType> _waypointTypes;
        private static IList<Waypoint> _waypoints;

        private static void Initialise()
        {
            _client = new MongoClient("mongodb://localhost");
            _database = _client.GetDatabase("Data");

            _waypointTypeCollection = _database.GetCollection<WaypointType>("WaypointTypes");
            _waypointTypes = _waypointTypeCollection.AsQueryable().ToList();
            if(_waypointTypes.Count == 0)
            {
                // Save types
                _waypointTypes = new List<WaypointType>()
                {
                    new WaypointType
                    {
                        Name = "Flappy Waypoint",
                        Type = TypeDefinition.Type1
                    },
                    new WaypointType
                    {
                        Name = "Glidey Waypoint",
                        Type = TypeDefinition.Type1
                    },
                    new WaypointType
                    {
                        Name = "Soaring Waypoint",
                        Type = TypeDefinition.Type2
                    }
                };

                _waypointTypeCollection.InsertMany(_waypointTypes);
            }

            _waypointCollection = _database.GetCollection<Waypoint>("Waypoints");
            _waypoints = new List<Waypoint>()
            {
                new Waypoint(-32.759965, 151.577419, 20.0, _waypointTypes[0]),
                new Waypoint(-32.760234, 151.577629, 20.0, _waypointTypes[1]),
                new Waypoint(-32.760309, 151.577513, 20.0, _waypointTypes[2]),
                new Waypoint(-32.760058, 151.577258, 20.0, _waypointTypes[1])
            };
        }

        public static void DoThings()
        {
            Initialise();
            
            DeleteThings();
            InsertThings();
            UpdateThings();
            ReadThings();
        }

        private static void UpdateThings()
        {
            var filter = Builders<Waypoint>.Filter.Eq("Latitude", -32.759965);
            var update = Builders<Waypoint>.Update.Set(w => w.AltitudeMetres, 40);
            UpdateResult updateResult = _waypointCollection.UpdateOne(filter, update);

            Console.WriteLine("Updated waypoint to double altitude");
        }

        private static void DeleteThings()
        {
            _waypointCollection.DeleteMany(new BsonDocument());
            Console.WriteLine("Deleted all waypoints");
        }

        private static void ReadThings()
        {
            Waypoint waypoint = _waypointCollection.AsQueryable().Where(w => w.Latitude == -32.759965).First();
            Console.WriteLine("Queried waypoint with ID " + waypoint.Id);

            WaypointType waypointType = waypoint.GetWaypointType(_database);
            Console.WriteLine("Queried waypoint type from separate collection: " + waypointType.Name);

            List<Waypoint> waypoints = _waypointCollection.AsQueryable().Where(w => w.Latitude == -32.759965 || w.Latitude == -32.760058).ToList();
            Console.WriteLine("Queried " + waypoints.Count + " other waypoints");
        }

        private static void InsertThings()
        {
            _waypointCollection.InsertMany(_waypoints);
            Console.WriteLine("Inserted " + _waypoints.Count + " waypoints");
        }
        
    }
}
