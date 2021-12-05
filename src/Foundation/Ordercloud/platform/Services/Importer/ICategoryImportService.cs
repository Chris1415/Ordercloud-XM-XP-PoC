using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{ 
    public interface ICategoryImportService
    {
        bool Import(string catalogId, Item catalogItem);
    }
}
