using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoConsole2Tests
{
    public class Address
    {
        [BsonElement("building")]
        public string Building { get; set; }

        [BsonElement("coord")]
        public List<Coordinates> Coord { get; set; }

        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("zipcode")]
        public string ZipCode { get; set; }

        [BsonElement("cuisine")]
        public string Cuisine {get; set;}

        [BsonElement("grades")]
        public List<Grade> Grades { get; set; }
    }
}
