using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface IProductReferenceService
    {
        Item GetProduct(Item productReference);
    }
}
