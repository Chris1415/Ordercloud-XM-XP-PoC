using BasicCompany.Foundation.Products.Ordercloud.Customer.Services.Importer;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace BasicCompany.Foundation.Products.Ordercloud.Customer
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICustomProductImportService, CustomProductImportService>();
        }
    }
}