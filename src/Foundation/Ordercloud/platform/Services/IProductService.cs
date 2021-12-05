using Sitecore.Data.Items;
using System.Collections.Generic;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface IProductService
    {
        Item GetProduct(string productId);
        string GetProductId(Item product);
        IList<Item> GetAllProducts();
        int ClearProductVariants(string productId);
    }
}
