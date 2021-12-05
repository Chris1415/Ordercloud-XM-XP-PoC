using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IProductService _productService;

        public CatalogService(IProductService productService)
        {
            _productService = productService;
        }

        public Item GetProductCatalog(string productId)
        {
            var product = _productService.GetProduct(productId);
            return GetProductCatalog(product);
        }

        public Item GetProductCatalog(Item product)
        {
            if (product == null)
            {
                return null;
            }

            Item parent = product.Parent;
            while (parent != null && !parent.TemplateID.Equals(Products.Templates.Catalog.ID))
            {
                parent = parent.Parent;
            }

            return parent;
        }

        public string GetCatalogId(Item catalog)
        {
            return catalog[Products.Templates.Catalog.Fields.Id];
        }
    }
}
