using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoConsole2Tests
{
    //Look here: https://docs.mongodb.org/getting-started/csharp/indexes/
    public class IndexTests
    {        
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public IndexTests()
        {

            _client = new MongoClient("mongodb://192.168.59.103:27017");
            _database = _client.GetDatabase("test");
        }

        [Fact]
        public async void Query1()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var keys = Builders<BsonDocument>.IndexKeys.Ascending("cuisine");
            await collection.Indexes.CreateOneAsync(keys);

            using (var cursor = await collection.Indexes.ListAsync())
            {
                var indexes = await cursor.ToListAsync();
                indexes.Should().Contain(index => index["name"] == "cuisine_1");
            }
        }
        

        [Fact]
        public async void Query2()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var keys = Builders<BsonDocument>.IndexKeys.Ascending("cuisine").Ascending("address.zipcode");
            //await collection.Indexes.CreateOneAsync(keys, new CreateIndexOptions { Unique = true});
            await collection.Indexes.CreateOneAsync(keys);
            await collection.Indexes.CreateOneAsync(keys, new CreateIndexOptions { Unique = false });

            using (var cursor = await collection.Indexes.ListAsync())
            {
                var indexes = await cursor.ToListAsync();

                var res = indexes.Any(e => e["name"] == "cuisine_1_address.zipcode_1");


                indexes.Should().Contain(index => index["name"] == "cuisine_1_address.zipcode_1");
            }
        }
    }
}
