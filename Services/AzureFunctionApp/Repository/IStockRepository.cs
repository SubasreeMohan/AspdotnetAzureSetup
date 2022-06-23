using AzureFunctionApp.Entities;
using AzureFunctionApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionApp.Repository
{
    public interface IStockRepository
    {
        Task<bool> CreateStock(Stock stock);

        Task<StockList> GetStockDetails(string CompanyCode, DateTime StartDate, DateTime EndDate);

        Task<bool> DeleteStock(string companycode);

        Task<IEnumerable<Stock>> GetStockDetails(string companycode);
    }
}
