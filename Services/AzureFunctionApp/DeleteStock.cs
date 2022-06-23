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
    public class DeleteStock
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<DeleteStock> _logger;

        public DeleteStock(IStockRepository stockRepository, ILogger<DeleteStock> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }
        [FunctionName("DeleteStock")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Stock/{companycode}")] HttpRequest req,
            ILogger log,string companycode)
        {
            IActionResult result;

            try
            {
                var stocktodelete = await _stockRepository.GetStockDetails(companycode);

                if (stocktodelete == null)
                {
                    _logger.LogWarning($"Stock with companycode : {companycode} doesn't exist.");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }

                await _stockRepository.DeleteStock(companycode);
                result = new StatusCodeResult(StatusCodes.Status204NoContent);
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
