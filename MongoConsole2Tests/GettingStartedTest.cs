using System;
using System.Threading.Tasks;
using MongoConsole2;
using Xunit;
using MongoDB.Driver;
using MongoDB.Bson;
using FluentAssertions;

namespace MongoConsole2Tests
{
    //https://docs.mongodb.org/getting-started/csharp/query/
    public class GettingStartedTest
    {

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public GettingStartedTest()
        {

            _client = new MongoClient("mongodb://192.168.59.103:27017");
            _database = _client.GetDatabase("test");
        }

        [Fact]
        public async void Query1(){
            var repo = new Repo();
            var result = await repo.Query1();

            result.Should().BeGreaterOrEqualTo(25359);
        }

        [Fact]
        public async void Query2()
        {
            var repo = new Repo();
            var result = await repo.Query2();

            result.Should().NotBeEmpty();
        }

        [Fact]
        public async void Query3()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("address.zipcode", "10075");
            var result = await collection.Find(filter).ToListAsync();
            result.Should().NotBeEmpty();
            result.Count.Should().Be(99);
        }

        [Fact]
        public async void Query4()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("grades.grade", "B");
            var result = await collection.Find(filter).ToListAsync();

            result.Count.Should().Be(8280);
        }
    }
}
