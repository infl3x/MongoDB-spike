using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class WaypointType
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public TypeDefinition Type { get; set; }
    }

    public enum TypeDefinition : int
    {
        Type1 = 10,
        Type2 = 20
    }
}
