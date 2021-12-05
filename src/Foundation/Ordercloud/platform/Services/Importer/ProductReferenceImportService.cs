using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class ProductReferenceImportService : BaseOrdercloudService, IProductReferenceImportService
    {
        private readonly IProductService _productService;
        public ProductReferenceImportService(
           IOrdercloudWebclient ordercloudWebclient,
           IProductService productService) : base(ordercloudWebclient)
        {
            _productService = productService;
        }

        public bool Import(string categoryId, string catalogId, Item categoryItem)
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            var productReferences = client.Categories.ListProductAssignmentsAsync(catalogId, categoryId).Result;

            foreach (var productReference in productReferences.Items)
            {
                string id = productReference.ProductID;
                var existingItem = categoryItem.Children.FirstOrDefault(element => element.Name.Equals(id));
                if (existingItem == null)
                {
                    existingItem = categoryItem.Add(id, new TemplateID(Products.Templates.ProductReference.ID));
                }

                Item referredProduct = _productService.GetProduct(id);
                if (referredProduct == null)
                {
                    continue;
                }

                string displayName = referredProduct.DisplayName;

                existingItem.Editing.BeginEdit();
                existingItem[Products.Templates.Base.Fields.DisplayName] = displayName;
                existingItem[Products.Templates.ProductReference.Fields.ProductReference] = referredProduct.ID.ToString();
                existingItem.Editing.EndEdit();
            }
            return true;
        }

        public bool ImportForCatalogRoot(string catalogId, Item catalogItem)
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            var productReferences = client.Catalogs.ListProductAssignmentsAsync(catalogId).Result;

            foreach (var productReference in productReferences.Items)
            {
                string id = productReference.ProductID;
                var productReferencesCategories = client.Categories.ListProductAssignmentsAsync(catalogId, productID: id).Result;
                if (productReferencesCategories.Items.Any())
                {
                    continue;
                }

                var existingItem = catalogItem.Children.FirstOrDefault(element => element.Name.Equals(id));
                if (existingItem == null)
                {
                    existingItem = catalogItem.Add(id, new TemplateID(Products.Templates.ProductReference.ID));
                }

                Item referredProduct = _productService.GetProduct(id);
                if (referredProduct == null)
                {
                    continue;
                }

                string displayName = referredProduct.DisplayName;

                existingItem.Editing.BeginEdit();
                existingItem[Products.Templates.Base.Fields.DisplayName] = displayName;
                existingItem[Products.Templates.ProductReference.Fields.ProductReference] = referredProduct.ID.ToString();
                existingItem.Editing.EndEdit();
            }

            Sitecore.Caching.CacheManager.ClearAllCaches();
            return true;
        }
    }
}
