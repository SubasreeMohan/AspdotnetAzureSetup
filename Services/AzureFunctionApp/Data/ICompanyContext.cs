using AzureFunctionApp.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Data
{
    public interface ICompanyContext
    {
        public IMongoCollection<Company> Company { get; }
    }
}
