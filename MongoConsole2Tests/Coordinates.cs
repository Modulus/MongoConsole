using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoConsole2Tests
{
    public class Coordinates
    {
        [BsonElement("0")]
        public double first { get; set; }

        [BsonElement("1")]
        public double second { get; set; }
    }
}
