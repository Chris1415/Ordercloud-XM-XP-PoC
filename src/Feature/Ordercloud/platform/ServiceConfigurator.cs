using BasicCompany.Feature.Ordercloud.Controller;
using BasicCompany.Feature.Ordercloud.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace BasicCompany.Feature.Ordercloud
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(OrdercloudUiController));
            serviceCollection.AddTransient<IOrdercloudUiService, OrdercloudUiService>();
        }
    }
}