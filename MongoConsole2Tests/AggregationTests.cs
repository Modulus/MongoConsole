using MongoDB.Driver;
using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using MongoDB.Bson;

namespace MongoConsole2Tests
{
    //See here: https://docs.mongodb.org/getting-started/csharp/aggregation/
    public class AggregationTests
    {
        
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public AggregationTests()
        {

            _client = new MongoClient("mongodb://192.168.59.103:27017");
            _database = _client.GetDatabase("test");
        }


        [Fact]
        public async void Query1()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var aggregate = collection.Aggregate().Group(new BsonDocument { { "_id", "$borough" }, { "count", new BsonDocument("$sum", 1) } });
            var results = await aggregate.ToListAsync();

            var expectedResults = new[]
            {
                BsonDocument.Parse("{ _id : 'Staten Island', count : 969 }"),
                BsonDocument.Parse("{ _id : 'Brooklyn', count : 6086 }"),
                BsonDocument.Parse("{ _id : 'Manhattan', count : 10259 }"),
                BsonDocument.Parse("{ _id : 'Queens', count : 5656 }"),
                BsonDocument.Parse("{ _id : 'Bronx', count : 2338 }"),
                BsonDocument.Parse("{ _id : 'Missing', count : 51 }")
            };
            results.Should().BeEquivalentTo(expectedResults);
        }

        [Fact]
        public async void Query2()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var aggregate = collection.Aggregate()
                .Match(new BsonDocument { { "borough", "Queens" }, { "cuisine", "Brazilian" } })
                .Group(new BsonDocument { { "_id", "$address.zipcode" }, { "count", new BsonDocument("$sum", 1) } });
            var results = await aggregate.ToListAsync();

            var expectedResults = new[]
            {
                BsonDocument.Parse("{ _id : '11368', count : 1 }"),
                BsonDocument.Parse("{ _id : '11106', count : 3 }"),
                BsonDocument.Parse("{ _id : '11377', count : 1 }"),
                BsonDocument.Parse("{ _id : '11103', count : 1 }"),
                BsonDocument.Parse("{ _id : '11101', count : 2 }")
            };
            results.Should().BeEquivalentTo(expectedResults);
        }

        /*
         * What we need, list all Quisine
         */
        [Fact]
        public async void Query3()
        {
            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var aggregate = collection.Aggregate()
                .Match(new BsonDocument ())
                .Group(new BsonDocument { { "_id", "$cuisine" }, { "count", new BsonDocument("$sum", 1) } });

            var result = await aggregate.ToListAsync();

            result.Count.Should().BeGreaterOrEqualTo(86);


            aggregate = collection.Aggregate()
                .Match(new BsonDocument())
                .Group(new BsonDocument { { "_id", "$cuisine" } });

            result = await aggregate.ToListAsync();

            result.Count.Should().BeGreaterOrEqualTo(86);

        }
    }
}
