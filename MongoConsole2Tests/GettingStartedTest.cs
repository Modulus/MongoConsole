using System;
using System.Threading.Tasks;
using MongoConsole2;
using Xunit;
using MongoDB.Driver;
using MongoDB.Bson;
using FluentAssertions;

namespace MongoConsole2Tests
{
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
        public async void Query1_Test(){
            var repo = new Repo();
            var result = await repo.Query1();

            result.Should().BeGreaterOrEqualTo(25359);
        }

        [Fact]
        public async void Query2_Test()
        {
                var repo = new Repo();
                var result = await repo.Query2();

                result.Should().NotBeEmpty();

        }
    }
}
