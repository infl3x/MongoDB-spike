using MongoDB.Bson;
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

        public Waypoint()
        {
        }

        public Waypoint(double latitude, double longitude, double altitudeMetres)
        {
            Latitude = latitude;
            Longitude = longitude;
            AltitudeMetres = altitudeMetres;
        }
    }
}
