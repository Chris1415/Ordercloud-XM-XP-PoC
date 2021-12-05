using BasicCompany.Foundation.Products.Ordercloud.Customer.Models;
using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Customer.Services.Importer
{
    public class CustomProductImportService : ProductImportService, ICustomProductImportService
    {
        public CustomProductImportService(
            IOrdercloudWebclient ordercloudWebclient,
            IProductVariantImportService productVariantImportService,
            ISpecService specService) : base(ordercloudWebclient, productVariantImportService, specService)
        {
        }

        protected override OrderCloudClient Client => base.Client;

        protected override void HandleXp<T>(T product, Sitecore.Data.Items.Item productItem)
        {
            base.HandleXp(product, productItem);
            var customProduct = product as MyProduct;
            productItem[Templates.MyProduct.Fields.FirstXP] = customProduct?.xp?.FirstXP ?? string.Empty;
        }
    }
}
