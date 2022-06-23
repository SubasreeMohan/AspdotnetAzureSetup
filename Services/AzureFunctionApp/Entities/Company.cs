using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Entities
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string CompanyCode { get; set; }
        [BsonRequired]
        public string CompanyName { get; set; }
        [BsonRequired]
        public string CompanyCEO { get; set; }
        [BsonRequired]
        public decimal CompanyTurnOver { get; set; }
        [BsonRequired]
        public string CompanyWebsite { get; set; }
        [BsonRequired]
        public string StockExchange { get; set; }


    }
}
