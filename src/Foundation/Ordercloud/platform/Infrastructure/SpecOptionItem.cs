using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Infrastructure
{
    public class SpecOptionItem
    {
        public void OnItemDeleting(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item deletingItem = Event.ExtractParameter(args, 0) as Item;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (deletingItem == null || !"master".Equals(deletingItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!deletingItem.TemplateID.Equals(Products.Templates.SpecOption.ID))
                return;

            if (deletingItem.Name.Equals("__Standard Values"))
            {
                return;
            }

            using (new DatabaseSwitcher(deletingItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var specOptionId = deletingItem[Products.Templates.SpecOption.Fields.Id];
                var specItem = deletingItem.Parent;
                var specId = specItem[Products.Templates.Spec.Fields.Id];

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task innerTask = Task.Run(() => ordercloudAsyncService.DeleteSpecOptionAsync(client, specId, specOptionId));
                innerTask.Wait();

                Task updateProductsTask = Task.Run(() => UpdateProducts(client, ordercloudAsyncService, deletingItem));
            }
        }

        public void OnItemSaving(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;
            var itemChanges = Event.ExtractParameter(args, 1) as ItemChanges;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (savedItem == null || !"master".Equals(savedItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!savedItem.TemplateID.Equals(Products.Templates.SpecOption.ID))
                return;

            using (new DatabaseSwitcher(savedItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();

                var specItem = savedItem.Parent;
                string specId = specItem[Products.Templates.Spec.Fields.Id];
                string specOptionId = savedItem[Products.Templates.SpecOption.Fields.Id];
                string oldSpecOptionId = itemChanges.FieldChanges[Products.Templates.SpecOption.Fields.Id]?.OriginalValue;
                string specOptionValue = savedItem[Products.Templates.SpecOption.Fields.Value];
                string oldSpecOptionValue = itemChanges.FieldChanges[Products.Templates.SpecOption.Fields.Value]?.OriginalValue;

                using (new EventDisabler())
                {
                    savedItem.Editing.BeginEdit();

                    if (oldSpecOptionId != specOptionId && !string.IsNullOrEmpty(oldSpecOptionId))
                    {
                        specOptionId = oldSpecOptionId;
                        savedItem[Products.Templates.Spec.Fields.Id] = specOptionId;
                    }

                    if (savedItem.Name != specOptionId && !string.IsNullOrEmpty(specOptionId))
                    {
                        savedItem.Name = specOptionId;
                    }
                    else if (savedItem.Name != specOptionId && !string.IsNullOrEmpty(savedItem.Name))
                    {
                        savedItem[Products.Templates.SpecOption.Fields.Id] = savedItem.Name;
                        specOptionId = savedItem.Name;
                    }

                    if (savedItem.DisplayName != specOptionValue && !string.IsNullOrEmpty(specOptionValue))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = specOptionValue;
                    }
                    else if (savedItem.DisplayName != specOptionValue && !string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.SpecOption.Fields.Value] = savedItem.DisplayName;
                        specOptionValue = savedItem.DisplayName;
                    }
                    else if (string.IsNullOrEmpty(specOptionValue) && string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = savedItem.Name;
                        savedItem[Products.Templates.SpecOption.Fields.Value] = savedItem.Name;
                        specOptionValue = savedItem.Name;
                    }

                    savedItem.Editing.EndEdit();

                    Sitecore.Caching.CacheManager.ClearAllCaches();

                    SpecOption specOption = new SpecOption()
                    {
                        ID = specOptionId,
                        Value = specOptionValue,
                        PriceMarkup = decimal.TryParse(savedItem[Products.Templates.SpecOption.Fields.PriceMarkup], out decimal res) ? res : 0.0M,
                        PriceMarkupType = Enum.TryParse(savedItem[Products.Templates.Spec.Fields.DefinesVariant], out PriceMarkupType priceMarkupRes) ? priceMarkupRes : PriceMarkupType.NoMarkup
                    };

                    Task<SpecOption> task = Task.Run(() => ordercloudAsyncService.CreateOrPatchSpecOptionAsync(client, specId, specOptionId, specOption));
                    task.Wait();

                    Task updateProductsTask = Task.Run(() => UpdateProducts(client, ordercloudAsyncService, savedItem));
                }
            }
        }

        private void UpdateProducts(OrderCloudClient client, IOrdercloudAsyncService ordercloudAsyncService, Item currentItem)
        {
            var productService = DependencyResolver.Current.GetService<IProductService>();
            var productVariantImportService = DependencyResolver.Current.GetService<IProductVariantImportService>();

            using (new DatabaseSwitcher(currentItem.Database))
            {
                var specItem = currentItem.Parent;
                var referrers = Globals.LinkDatabase.GetReferrers(specItem);
                foreach (var referrer in referrers)
                {
                    var sourceItem = referrer.GetSourceItem();
                    if (sourceItem.TemplateID != Products.Templates.Product.ID)
                    {
                        continue;
                    }

                    string productId = productService.GetProductId(sourceItem);
                    Task addTask = Task.Run(() => ordercloudAsyncService.GenerateProductVariants(client, productId, true));
                    addTask.Wait();
                    productService.ClearProductVariants(productId);
                    productVariantImportService.Import(sourceItem);
                }
            }
        }
    }
}
