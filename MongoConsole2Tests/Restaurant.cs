using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoConsole2Tests
{
    public class Restaurant
    {
        private Address adreess;

        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }
      
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("restaurant_id")]
        public string RestaurantId { get; set; }
    }
}
