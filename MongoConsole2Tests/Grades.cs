using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoConsole2Tests
{
    public class Grade
    {
        [BsonElement("grade")]
        public string Letter { get; set; }

        [BsonElement("score")]
        public int? Score { get; set; }

        [BsonElement("date")]
        [BsonDateTimeOptions(DateOnly = false)]
        public DateTime Date { get; set; }
    }
}
