using AzureFunctionApp.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Data
{
    public class CompanyContext : ICompanyContext
    {
        public CompanyContext(IConfiguration Config)
        {
            var client = new MongoClient(Config.GetValue<string>("Values:ConnectionString"));
            var database = client.GetDatabase(Config.GetValue<string>("Values:DatabaseName"));
            Company = database.GetCollection<Company>(Config.GetValue<string>("Values:CompanyCollectionName"));
            var collections = database.GetCollection<Company>(Config.GetValue<string>("Values:CompanyCollectionName"));
            var Options = new CreateIndexOptions { Unique = true };
            collections.Indexes.CreateOne("{CompanyCode : 1}", Options);

        }

        public IMongoCollection<Company> Company { get; }
    }
}
