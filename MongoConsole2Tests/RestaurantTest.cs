using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace MongoConsole2Tests
{
    public class RestaurantTest
    {
        
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public RestaurantTest()
        {

            _client = new MongoClient("mongodb://192.168.59.103:27017");
            _database = _client.GetDatabase("test");
        }


        [Fact]
        public async void TestMethod1()
        {
            var collection = _database.GetCollection<Restaurant>("restaurants");
            var filter = new BsonDocument();

            var result = await collection.Find(filter).ToListAsync();
            result.Should().NotBeEmpty();
           
        }
    }
}
