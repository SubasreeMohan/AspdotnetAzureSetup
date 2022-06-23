using AzureFunctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionApp.Model
{
    public class StockList
    {
        public IEnumerable<Stock> stock { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Average { get; set; }
    }
}
