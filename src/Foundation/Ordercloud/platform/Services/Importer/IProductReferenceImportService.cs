using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public interface IProductReferenceImportService
    {
        bool Import(string categoryId, string catalogId, Item categoryItem);
        bool ImportForCatalogRoot(string catalogId, Item catalogItem);
    }
}
