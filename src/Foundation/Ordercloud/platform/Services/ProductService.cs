using Sitecore;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class ProductService : IProductService
    {
        public Item GetProduct(string productId)
        {
            var root = Context.Database.GetItem(Products.Constants.Global.ProductsRootItemId);
            return root?.Children.FirstOrDefault(element => element.Name.Equals(productId));
        }

        public string GetProductId(Item product)
        {
            return product?.Fields[Products.Templates.ProductData.Fields.Id]?.Value ?? string.Empty;
        }

        public IList<Item> GetAllProducts()
        {
            var root = Context.Database.GetItem(Products.Constants.Global.ProductsRootItemId);
            return root?.Children.ToList();
        }
        public int ClearProductVariants(string productId)
        {
            var product = GetProduct(productId);
            return product != null
                ? product.DeleteChildren()
                : 0;
        }
    }
}
