using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface ICatalogService
    {
        Item GetProductCatalog(string productId);
        Item GetProductCatalog(Item product);
        string GetCatalogId(Item catalog);
    }
}
