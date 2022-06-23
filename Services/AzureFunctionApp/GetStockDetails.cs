using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionApp.Repository;

namespace AzureFunctionApp
{
    public class GetStockDetails
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<GetStockDetails> _logger;

        public GetStockDetails(IStockRepository stockRepository, ILogger<GetStockDetails> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        [FunctionName("GetStockDetails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Stock/{companycode}/{startdate}/{enddate}")] HttpRequest req,
            ILogger log,string companycode,DateTime startdate, DateTime enddate)
        {
            IActionResult result;

            try
            {
                var stockList = await _stockRepository.GetStockDetails(companycode, startdate, enddate);

                if (stockList == null)
                {
                    _logger.LogWarning($"Stock with companycode: {companycode} doesn't exist.");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }

                result = new OkObjectResult(stockList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception thrown: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
