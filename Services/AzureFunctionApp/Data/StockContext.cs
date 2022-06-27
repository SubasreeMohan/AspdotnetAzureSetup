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
            var client = new MongoClient("mongodb://companydb:7JEyCOC8RZW34ufH6nzlTeblDRjjeB0MwQZ8jLXDMURAwW4Pv6rMe0qxY2tIKA19rRBlbVcLf9Sk1usvODDZjg==@companydb.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@companydb@");
            var database = client.GetDatabase("CompanyDB");
            Stock = database.GetCollection<Stock>("Stock");
        }

        public IMongoCollection<Stock> Stock { get; }
    }
}
