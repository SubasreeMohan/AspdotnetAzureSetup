using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using AzureFunctionApp.Entities;

namespace AzureFunctionApp.Data
{
    public interface IStockContext
    {
        public IMongoCollection<Stock> Stock { get; }
    }
}
