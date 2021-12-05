using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace BasicCompany.Foundation.Products.Ordercloud
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICatalogImportService, CatalogImportService>();
            serviceCollection.AddTransient<ICategoryImportService, CategoryImportService>();
            serviceCollection.AddTransient<IProductImportService, ProductImportService>();
            serviceCollection.AddTransient<IBuyersImportService, BuyersImportService>();
            serviceCollection.AddTransient<ISupplierImportService, SupplierImportService>();
            serviceCollection.AddTransient<IBuyerService, BuyerService>();
            serviceCollection.AddTransient<ISellersImportService, SellersImportService>();
            serviceCollection.AddTransient<ICategoryService, CategoryService>();
            serviceCollection.AddTransient<ICatalogService, CatalogService>();
            serviceCollection.AddTransient<ISpecService, SpecService>();
            serviceCollection.AddTransient<IProductService, ProductService>();
            serviceCollection.AddTransient<IProductReferenceImportService, ProductReferenceImportService>();
            serviceCollection.AddTransient<IProductVariantSpecImportService, ProductVariantSpecImportService>();
            serviceCollection.AddTransient<IProductVariantImportService, ProductVariantImportService>();
            serviceCollection.AddTransient<IOrderService, OrderService>();
            serviceCollection.AddTransient<IProductReferenceService, ProductReferenceService>();
            serviceCollection.AddTransient<IOrdercloudWebclient, OrdercloudWebclient>();
            serviceCollection.AddTransient<IContentPagesService, ContentPagesService>();

            serviceCollection.AddSingleton<IOrdercloudAsyncService, OrdercloudAsyncService>();
            serviceCollection.AddSingleton<IOrdercloudSettingsRepository, OrdercloudSettingsRepository>();
        }
    }
}