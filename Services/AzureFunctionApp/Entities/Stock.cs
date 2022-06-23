using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Entities
{
    public class Stock
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string CompanyCode { get; set; }
        [BsonRequired]
        public decimal StockPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
