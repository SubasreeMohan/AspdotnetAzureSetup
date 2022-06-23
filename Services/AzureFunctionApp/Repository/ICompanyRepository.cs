using AzureFunctionApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionApp.Repository
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompanies();
        Task<Company> GetCompanyDetails(string CompanyCode);
        Task<List<string>> CreateCompany(Company company);
        Task<bool> DeleteCompany(string CompanyCode);
    }
}
