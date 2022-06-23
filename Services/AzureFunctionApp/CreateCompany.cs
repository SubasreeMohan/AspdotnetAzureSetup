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
    public class CreateCompany
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CreateCompany> _logger;

        public CreateCompany(ICompanyRepository companyRepository, ILogger<CreateCompany> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        [FunctionName("CreateCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = "Company")] HttpRequest req,
            ILogger log)
        {
            IActionResult result;

            try
            {
                var incomingRequest = await new StreamReader(req.Body).ReadToEndAsync();

                var companyRequest = JsonConvert.DeserializeObject<Company>(incomingRequest);

                var company = new Company
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CompanyCode = companyRequest.CompanyCode,
                    CompanyName = companyRequest.CompanyName,
                    CompanyCEO = companyRequest.CompanyCEO,
                    CompanyTurnOver = companyRequest.CompanyTurnOver,
                    CompanyWebsite = companyRequest.CompanyWebsite,
                    StockExchange = companyRequest.StockExchange
                };

                await _companyRepository.CreateCompany(company);

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
