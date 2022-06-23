using AzureFunctionApp.Data;
using AzureFunctionApp.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionApp.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ICompanyContext _context;

        public CompanyRepository(ICompanyContext context)
        {
            _context = context;
        }

        public async Task<List<string>> CreateCompany(Company company)
        {
            List<string> validationdata = ValidateCompanyData(company);
            if (validationdata.Count == 0)
            {
                await _context.Company.InsertOneAsync(company);
            }
            return validationdata;
        }

        private List<string> ValidateCompanyData(Company company)
        {
            List<string> validationdata = new List<string>();
            if (company.CompanyTurnOver < 10000000)
            {
                validationdata.Add("Company Turn Over should be greater than 10000000");
            }
            Company c = _context.Company.Find(p => p.CompanyCode == company.CompanyCode).FirstOrDefault();
            if (c != null)
            {
                validationdata.Add("CompanyCode should be unique");
            }
            return validationdata;
        }

        public async Task<bool> DeleteCompany(string CompanyCode)
        {
            FilterDefinition<Company> filter = Builders<Company>.Filter.Eq(p => p.CompanyCode, CompanyCode);
            DeleteResult deleteResult = await _context.Company.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _context.Company.Find(p => true).ToListAsync();
        }

        public async Task<Company> GetCompanyDetails(string CompanyCode)
        {
            return await _context.Company.Find(p => p.CompanyCode == CompanyCode).FirstOrDefaultAsync();
        }
    }
}
