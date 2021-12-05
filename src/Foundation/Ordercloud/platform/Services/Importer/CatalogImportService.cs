using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class CatalogImportService : BaseOrdercloudService, ICatalogImportService
    {
        private readonly ICategoryImportService _categoryImportService;
        private readonly IProductReferenceImportService _productReferenceImportService;

        public CatalogImportService(
            IOrdercloudWebclient ordercloudWebclient,
            ICategoryImportService categoryImportService,
            IProductReferenceImportService productReferenceImportService) : base(ordercloudWebclient)
        {
            _categoryImportService = categoryImportService;
            _productReferenceImportService = productReferenceImportService;
        }

        public bool Import()
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            var catalogs = client.Catalogs.ListAsync().Result;

            var root = Context.Database.GetItem(Products.Constants.Global.CatalogRootItemId);
            if (root == null)
            {
                return false;
            }

            foreach (var catalog in catalogs.Items)
            {
                string displayName = catalog.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string id = catalog.ID;
                string description = catalog.Description;
                bool isActive = catalog.Active;
                string OwnerId = catalog.OwnerID;
                int categoryCount = catalog.CategoryCount;

                var existingItem = root.Children.FirstOrDefault(element => element.Name.Equals(id));
                if (existingItem == null)
                {
                    using (new EventDisabler())
                    {
                        existingItem = root.Add(id, new TemplateID(Products.Templates.Catalog.ID));
                    }
                }

                using (new EventDisabler())
                {
                    existingItem.Editing.BeginEdit();
                    existingItem[Products.Templates.Catalog.Fields.Name] = displayName;
                    existingItem[Products.Templates.Catalog.Fields.Id] = id;
                    existingItem[Products.Templates.Base.Fields.DisplayName] = displayName;
                    existingItem[Products.Templates.Catalog.Fields.Description] = description;
                    existingItem[Products.Templates.Catalog.Fields.IsActive] = isActive ? "1" : "0";
                    existingItem[Products.Templates.Catalog.Fields.CategoryCount] = categoryCount.ToString();
                    existingItem[Products.Templates.Catalog.Fields.OwnerId] = OwnerId;
                    existingItem.Editing.EndEdit();
                }

                bool categoryImportOk = _categoryImportService.Import(id, existingItem);

                bool productReferenceOk = _productReferenceImportService.ImportForCatalogRoot(id, existingItem);
            }

            Sitecore.Caching.CacheManager.ClearAllCaches();
            return true;
        }
    }
}
