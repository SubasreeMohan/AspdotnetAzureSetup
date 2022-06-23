using AzureFunctionApp.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Data
{
    public class StockContext : IStockContext
    {
        public StockContext(IConfiguration Config)
        {
            var client = new MongoClient(Config.GetValue<string>("Values:ConnectionString"));
            var database = client.GetDatabase(Config.GetValue<string>("Values:DatabaseName"));
            Stock = database.GetCollection<Stock>(Config.GetValue<string>("Values:StockCollectionName"));
        }

        public IMongoCollection<Stock> Stock { get; }
    }
}
