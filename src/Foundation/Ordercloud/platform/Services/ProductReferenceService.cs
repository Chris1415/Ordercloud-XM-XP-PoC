using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class ProductReferenceService : IProductReferenceService
    {
        public Item GetProduct(Item productReference)
        {
            string productItemId = productReference?.Fields[Products.Templates.ProductReference.Fields.ProductReference]?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(productItemId))
            {
                return null;
            }

            return Sitecore.Context.Database.GetItem(productItemId);
        }
    }
}
