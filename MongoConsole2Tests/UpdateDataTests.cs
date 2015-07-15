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

        [Fact]
        public async void Query3()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("address.zipcode", "10016") & builder.Eq("cuisine", "Other");
            var update = Builders<BsonDocument>.Update
                .Set("cuisine", "Category To Be Determined")
                .CurrentDate("lastModified");
            var result = await collection.UpdateManyAsync(filter, update);
            result.MatchedCount.Should().Be(20);
            if (result.IsModifiedCountAvailable)
            {
                result.ModifiedCount.Should().Be(20);
            }


            filter = builder.Eq("address.zipcode", "10016") & builder.Eq("cuisine", "Category To Be Determined");
            update = Builders<BsonDocument>.Update
                .Set("cuisine", "Other")
                .CurrentDate("lastModified");
            result = await collection.UpdateManyAsync(filter, update);

            result.MatchedCount.Should().Be(20);
            if (result.IsModifiedCountAvailable)
            {
                result.ModifiedCount.Should().Be(20);
            }
        }

        [Fact(Skip = "Will actually delete data")]
        public async void Query4()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("borough", "Manhattan");
            var result = await collection.DeleteManyAsync(filter);

            result.DeletedCount.Should().Be(10259);

        }

        [Fact(Skip = "Will delete all documents :O")]
        public async void Query5()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = new BsonDocument();
            var result = await collection.DeleteManyAsync(filter);

            result.DeletedCount.Should().Be(25359);
        }

        [Fact(Skip="Will delete the database")]
        public async void Query6()
        {
            await _database.DropCollectionAsync("restaurants");
            using (var cursor = await _database.ListCollectionsAsync())
            {
                var collections = await cursor.ToListAsync();
                collections.Should().NotContain(document => document["name"] == "restaurants");
            }
        }
    }
}
