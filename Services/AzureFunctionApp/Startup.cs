using AzureFunctionApp;
using AzureFunctionApp.Data;
using AzureFunctionApp.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.IO;
using System.Security.Authentication;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(config);

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(config["ConnectionString"]));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            builder.Services.AddSingleton((s) => new MongoClient(settings));
            builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
            builder.Services.AddTransient<IStockRepository, StockRepository>();
            builder.Services.AddTransient<ICompanyContext, CompanyContext>();
            builder.Services.AddTransient<IStockContext, StockContext>();
        }
    }
}
