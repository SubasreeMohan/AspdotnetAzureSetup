using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AzureFunctionApp.Data;
using AzureFunctionApp.Entities;
using AzureFunctionApp.Model;


namespace AzureFunctionApp.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly IStockContext _context;

        public StockRepository(IStockContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateStock(Stock stock)
        {
            try
            {
                await _context.Stock.InsertOneAsync(stock);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteStock(string companycode)
        {
            FilterDefinition<Stock> filter = Builders<Stock>.Filter.Eq(p => p.CompanyCode, companycode);
            DeleteResult deleteResult = await _context.Stock.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<StockList> GetStockDetails(string CompanyCode, DateTime StartDate, DateTime EndDate)
        {
            var stock = await _context.Stock.Find(p => p.CompanyCode == CompanyCode).ToListAsync();
            StockList stockList = new StockList();
            stockList.stock = stock.Where(x => x.CreatedDate.Date >= StartDate.Date && x.CreatedDate.Date <= EndDate.Date).ToList();
            stockList.Min = stockList.stock.Min(P => P.StockPrice);
            stockList.Max = stockList.stock.Max(P => P.StockPrice);
            stockList.Average = stockList.stock.Average(P => P.StockPrice);

            return stockList;
        }

        public async Task<IEnumerable<Stock>> GetStockDetails(string companycode)
        {
            return await _context.Stock.Find(p => p.CompanyCode == companycode).ToListAsync();
        }
    }
}
