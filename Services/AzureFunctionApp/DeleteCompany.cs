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
    public class DeleteCompany
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<DeleteCompany> _logger;

        public DeleteCompany(ICompanyRepository companyRepository, ILogger<DeleteCompany> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        [FunctionName("DeleteCompany")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Company/{companycode}")] HttpRequest req,
            ILogger log,string companycode)
        {
            IActionResult result;

            try
            {
                var companytodelete = await _companyRepository.GetCompanyDetails(companycode);

                if (companytodelete == null)
                {
                    _logger.LogWarning($"Company with companycode : {companycode} doesn't exist.");
                    result = new StatusCodeResult(StatusCodes.Status404NotFound);
                }

                await _companyRepository.DeleteCompany(companycode);
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
