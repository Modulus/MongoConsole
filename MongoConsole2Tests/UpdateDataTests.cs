using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using MongoDB.Driver;
using MongoDB.Bson;
using FluentAssertions;

namespace MongoConsole2Tests
{
    public class UpdateDataTests
    {

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public UpdateDataTests()
        {

            _client = new MongoClient("mongodb://192.168.59.103:27017");
            _database = _client.GetDatabase("test");
        }


        [Fact]
        public async void Query1()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("name", "Juni");
            var update = Builders<BsonDocument>.Update
                .Set("cuisine", "American (New)")
                .CurrentDate("lastModified");
            var result = await collection.UpdateOneAsync(filter, update);
            
            
            result.MatchedCount.Should().Be(1);
            if (result.IsModifiedCountAvailable)
            {
                result.ModifiedCount.Should().Be(1);
            }

            //Set it back
            update = Builders<BsonDocument>.Update
            .Set("cuisine", "American")
            .CurrentDate("lastModified");
            result = await collection.UpdateOneAsync(filter, update);


            result.MatchedCount.Should().Be(1);
            if (result.IsModifiedCountAvailable)
            {
                result.ModifiedCount.Should().Be(1);
            }
        }

        [Fact]
        public async void Query2()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("restaurant_id", "41156888");
            var update = Builders<BsonDocument>.Update.Set("address.street", "East 31st Street");
            var result = await collection.UpdateOneAsync(filter, update);

            result.MatchedCount.Should().Be(1);
            if (result.IsModifiedCountAvailable)
            {
                result.ModifiedCount.Should().Be(1);
            }

            update = Builders<BsonDocument>.Update.Set("address.street", "East 31 Street");
            result = await collection.UpdateOneAsync(filter, update);

            result.MatchedCount.Should().Be(1);
            if (result.IsModifiedCountAvailable)
            {
                result.ModifiedCount.Should().Be(1);
            }
        }
    }
}
