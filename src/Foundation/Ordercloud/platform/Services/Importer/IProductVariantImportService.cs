using OrderCloud.SDK;
using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public interface IProductVariantImportService
    {
        bool Import(Item product);
    }
}
