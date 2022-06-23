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
    public class GetCompanyByCompanyCode
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<GetCompanyByCompanyCode> _logger;

        public GetCompanyByCompanyCode(ICompanyRepository companyRepository, ILogger<GetCompanyByCompanyCode> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }
        [FunctionName("GetCompanyByCompanyCode")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Company/{companycode}")] HttpRequest req,
            ILogger log,string companycode)
        {
            IActionResult result;

            try
            {
                var company = await _companyRepository.GetCompanyDetails(companycode);

                if (company == null)
                {
                    _logger.LogWarning("No Company details found!");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }

                result = new OkObjectResult(company);
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
