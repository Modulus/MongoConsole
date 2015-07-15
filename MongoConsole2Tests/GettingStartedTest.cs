using System;
using System.Threading.Tasks;
using MongoConsole2;
using Xunit;
using MongoDB.Driver;
using MongoDB.Bson;
using FluentAssertions;
using FluentAssertions.Collections;

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

        [Fact]
        public async void Query5()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Gt("grades.score", 30);
            var result = await collection.Find(filter).ToListAsync();

            result.Count.Should().Be(1959);

            filter = Builders<BsonDocument>.Filter.Lt("grades.score", 10);
            result = await collection.Find(filter).ToListAsync();
            result.Count.Should().Be(19065);

        }

        [Fact]
        public async void Query6()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("cuisine", "Italian") & builder.Eq("address.zipcode", "10075");
            var result = await collection.Find(filter).ToListAsync();

            result.Count.Should().Be(15);


            filter = builder.Eq("cuisine", "Italian") | builder.Eq("address.zipcode", "10075");
            result = await collection.Find(filter).ToListAsync();

            result.Count.Should().Be(1153);
        }

        [Fact]
        public async void Query7()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Ascending("borough").Ascending("address.zipcode");
            var result = await collection.Find(filter).Sort(sort).ToListAsync();

            Func<BsonDocument, BsonDocument> keyFunc = document => new BsonDocument { { "borough", document["borough"] }, { "address.zipcode", document.GetValue("address.zipcode", "") } };
            //IsInAscendingOrder(result, keyFunc).Should().BeTrue();

            //result.Should().BeInAscendingOrder();
        }
    }
}
