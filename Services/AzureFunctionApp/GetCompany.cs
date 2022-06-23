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
    public class GetCompany
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<GetCompany> _logger;

        public GetCompany(ICompanyRepository companyRepository, ILogger<GetCompany> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }
        [FunctionName("GetCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Company")] HttpRequest req,
            ILogger log)
        {
            IActionResult result;

            try
            {
                var companies = await _companyRepository.GetAllCompanies();

                if (companies == null)
                {
                    _logger.LogWarning("No Company details found!");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }

                result = new OkObjectResult(companies);
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
