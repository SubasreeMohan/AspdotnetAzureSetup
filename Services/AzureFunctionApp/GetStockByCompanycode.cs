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
    public class GetStockByCompanycode
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<GetStockByCompanycode> _logger;

        public GetStockByCompanycode(IStockRepository stockRepository, ILogger<GetStockByCompanycode> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        [FunctionName("GetStockByCompanycode")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Stock/{companycode}")] HttpRequest req,
            ILogger log,string companycode)
        {
            IActionResult result;

            try
            {
                var stockList = await _stockRepository.GetStockDetails(companycode);

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
