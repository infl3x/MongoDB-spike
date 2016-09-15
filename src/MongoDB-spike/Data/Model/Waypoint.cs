using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Waypoint
    {
        public ObjectId Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AltitudeMetres { get; set; }

        /// <summary>
        /// Reference documents are represented via an intermediary type called MongoDBRef, which just holds all the info
        /// needed to resolve the reference (ID, collection name, and DB name). The pattern is explained better in this guy's
        /// blog post: https://chrisbitting.com/2015/03/24/mongodb-linking-records-documents-using-mongodbref/
        /// </summary>
        public MongoDBRef WaypointType { get; set; }

        public Waypoint()
        {
        }

        public Waypoint(double latitude, double longitude, double altitudeMetres, WaypointType waypointType)
        {
            Latitude = latitude;
            Longitude = longitude;
            AltitudeMetres = altitudeMetres;
            WaypointType = new MongoDBRef("WaypointTypes", waypointType.Id);
        }

        /// <summary>
        /// It's possible to then write referrence resolution helper methods, like this one to resolve foreign references.
        /// There are a couple of frameworks which appear to assist with some of this stuff, one of which is this guy's 
        /// package: https://github.com/RobThree/MongoRepository - looks like it simplifies all this stuff heaps.
        /// </summary>
        /// <param name="mongoDatabase"></param>
        /// <returns></returns>
        public WaypointType GetWaypointType(IMongoDatabase mongoDatabase)
        {
            if (WaypointType == null)
                return null;

            IMongoCollection<WaypointType> waypointCollection = mongoDatabase.GetCollection<WaypointType>(WaypointType.CollectionName);
            return waypointCollection.AsQueryable().Where(w => w.Id == WaypointType.Id).First();
        }
    }
}
