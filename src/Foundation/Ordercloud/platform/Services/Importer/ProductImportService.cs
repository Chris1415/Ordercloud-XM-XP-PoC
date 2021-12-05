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
    public class ProductImportService : BaseOrdercloudService, IProductImportService
    {
        private readonly IProductVariantImportService _productVariantImportService;
        private readonly ISpecService _specService;

        public ProductImportService(
            IOrdercloudWebclient ordercloudWebclient,
            IProductVariantImportService productVariantImportService,
            ISpecService specService) : base(ordercloudWebclient)
        {
            _productVariantImportService = productVariantImportService;
            _specService = specService;
        }

        protected virtual OrderCloudClient Client => _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

        protected virtual void HandleXp<T>(T product, Item productItem)
        {
        }

        public virtual bool Import<T>() where T : Product
        {
            var client = Client;
            // TODO REMOVE HARDCODED 
            var products = client.Products.ListAsync<T>(pageSize: 100).Result;

            var root = Context.Database.GetItem(Products.Constants.Global.ProductsRootItemId);
            if (root == null)
            {
                return false;
            }

            foreach (var product in products.Items)
            {
                string displayName = product.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string id = product.ID;
                string description = product.Description;
                bool isActive = product.Active;
                decimal? height = product.ShipHeight;
                decimal? weight = product.ShipWeight;
                decimal? width = product.ShipWidth;
                decimal? length = product.ShipLength;
                bool allsuppliersCanSell = product.AllSuppliersCanSell;
                bool autoForward = product.AutoForward;
                int? quantityMultiplier = product.QuantityMultiplier;

                var productSpecsAssignments = client.Specs.ListProductAssignmentsAsync(id, "ProductId").Result;
                var productSpecs = productSpecsAssignments.Items.Select(element => _specService.GetSpec(element.SpecID)?.ID?.ToString());

                string defaultSupplierId = product.DefaultSupplierID;
                var inventory = product.Inventory;
                string shipFromAddressId = product.ShipFromAddressID;
                string defaultPriceScheduleId = product.DefaultPriceScheduleID;
                string ownerId = product.OwnerID;

                var existingItem = root.Children.FirstOrDefault(element => element.Name.Equals(id));
                if (existingItem == null)
                {
                    using (new EventDisabler())
                    {
                        existingItem = root.Add(id, new TemplateID(Products.Templates.Product.ID));
                    }
                }

                using (new EventDisabler())
                {
                    existingItem.Editing.BeginEdit();
                    existingItem[Products.Templates.ProductData.Fields.Name] = name;
                    existingItem[Products.Templates.Base.Fields.DisplayName] = displayName;
                    existingItem[Products.Templates.ProductData.Fields.Id] = id;
                    existingItem[Products.Templates.ProductData.Fields.Description] = description;
                    existingItem[Products.Templates.ProductData.Fields.IsActive] = isActive ? "1" : "0";
                    existingItem[Products.Templates.ProductData.Fields.Height] = height.HasValue ? height.Value.ToString() : string.Empty;
                    existingItem[Products.Templates.ProductData.Fields.Width] = width.HasValue ? width.Value.ToString() : string.Empty;
                    existingItem[Products.Templates.ProductData.Fields.Length] = length.HasValue ? length.Value.ToString() : string.Empty;
                    existingItem[Products.Templates.ProductData.Fields.Weight] = weight.HasValue ? weight.Value.ToString() : string.Empty;
                    existingItem[Products.Templates.Product.Fields.AllSupplierCanSell] = allsuppliersCanSell ? "1" : "0";
                    existingItem[Products.Templates.Product.Fields.AutoForward] = autoForward ? "1" : "0";
                    existingItem[Products.Templates.Product.Fields.QuantityMultiplier] = quantityMultiplier.ToString();
                    existingItem[Products.Templates.Product.Fields.ProductSpecs] = string.Join("|", productSpecs);

                    HandleXp(product, existingItem);
                    existingItem.Editing.EndEdit();
                }

                bool variantsImported = _productVariantImportService.Import(existingItem);
            }

            Sitecore.Caching.CacheManager.ClearAllCaches();
            return true;
        }
    }
}
