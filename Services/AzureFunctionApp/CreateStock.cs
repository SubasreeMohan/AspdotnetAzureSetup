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
using AzureFunctionApp.Entities;
using MongoDB.Bson;

namespace AzureFunctionApp
{
    public class CreateStock
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<CreateStock> _logger;

        public CreateStock(IStockRepository stockRepository, ILogger<CreateStock> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        [FunctionName("CreateStock")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "Stock")] HttpRequest req,
            ILogger log)
        {
            IActionResult result;

            try
            {
                var incomingRequest = await new StreamReader(req.Body).ReadToEndAsync();

                var stockRequest = JsonConvert.DeserializeObject<Stock>(incomingRequest);

                var stock = new Stock
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CompanyCode = stockRequest.CompanyCode,
                    StockPrice = stockRequest.StockPrice,
                    CreatedDate = stockRequest.CreatedDate
                };

                await _stockRepository.CreateStock(stock);

                result = new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error. Exception: {ex.Message}");
                result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
