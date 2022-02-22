using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Links;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Infrastructure
{
    public class ProductItem
    {
        public void OnItemDeleting(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item deletingItem = Event.ExtractParameter(args, 0) as Item;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (deletingItem == null || !"master".Equals(deletingItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!deletingItem.TemplateID.Equals(Products.Templates.Product.ID))
                return;

            if (deletingItem.Name.Equals("__Standard Values"))
            {
                return;
            }

            using (new DatabaseSwitcher(deletingItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var productService = DependencyResolver.Current.GetService<IProductService>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var productId = productService.GetProductId(deletingItem);

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task innerTask = Task.Run(() => ordercloudAsyncService.DeleteProductAsync(client, productId));
                innerTask.Wait();
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

            if (!savedItem.TemplateID.Equals(Products.Templates.Product.ID))
                return;

            using (new DatabaseSwitcher(savedItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

                var partialProduct = new PartialProduct()
                {
                    Name = savedItem[Products.Templates.ProductData.Fields.Name],
                    Description = savedItem[Products.Templates.ProductData.Fields.Description],
                    Active = ((CheckboxField)savedItem.Fields[Products.Templates.ProductData.Fields.IsActive])?.Checked ?? false
                };

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                var productService = DependencyResolver.Current.GetService<IProductService>();
                string productId = productService.GetProductId(savedItem);
                string oldProductId = itemChanges.FieldChanges[Products.Templates.ProductData.Fields.Id]?.OriginalValue;
                string productName = itemChanges.FieldChanges[Products.Templates.ProductData.Fields.Name]?.Value;
                Task<Product> task = Task.Run(() => ordercloudAsyncService.PatchProductAsync(client, productId, partialProduct));
                task.Wait();

                using (new EventDisabler())
                {
                    savedItem.Editing.BeginEdit();
                    if (!string.IsNullOrEmpty(oldProductId))
                    {
                        if (productId != oldProductId)
                        {
                            savedItem[Products.Templates.ProductData.Fields.Id] = oldProductId;
                            savedItem.Name = oldProductId;
                        }
                        else
                        {
                            savedItem.Name = productId;
                        }
                    }

                    if (!string.IsNullOrEmpty(productName))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = productName;
                    }
                  

                    savedItem.Editing.EndEdit();
                }

                var productSpecs = itemChanges.FieldChanges[Products.Templates.Product.Fields.ProductSpecs]?.Value?.Split('|').Select(eleemnt => Context.Database.GetItem(eleemnt));
                var oldProductSpecs = ((MultilistField)savedItem.Fields[Products.Templates.Product.Fields.ProductSpecs])?.GetItems();

                // TO DELETE
                if (oldProductSpecs != null && productSpecs != null)
                {
                    var remainingOldProductSpecs = oldProductSpecs.Except(productSpecs);
                    foreach (var remainingOldProductSpec in remainingOldProductSpecs)
                    {
                        if (remainingOldProductSpec == null)
                        {
                            continue;
                        }

                        string specId = remainingOldProductSpec[Products.Templates.Spec.Fields.Id];
                        Task deleteTask = Task.Run(() => ordercloudAsyncService.DeleteSpecProductAssignmentAsync(client, specId, productId));
                        deleteTask.Wait();
                    }

                    // TO ADD
                    var remainingNewProductSpecs = productSpecs.Except(oldProductSpecs);
                    foreach (var remainingNewProductSpec in remainingNewProductSpecs)
                    {
                        if (remainingNewProductSpec == null)
                        {
                            continue;
                        }

                        string specId = remainingNewProductSpec[Products.Templates.Spec.Fields.Id];
                        var specProductAssignment = new SpecProductAssignment()
                        {
                            SpecID = specId,
                            ProductID = productId,
                        };
                        Task addTask = Task.Run(() => ordercloudAsyncService.AddSpecProductAssignmentAsync(client, specProductAssignment));
                        addTask.Wait();
                    }

                    if (remainingNewProductSpecs.Any() || remainingOldProductSpecs.Any())
                    {
                        Task addTask = Task.Run(() => ordercloudAsyncService.GenerateProductVariants(client, productId, true));
                        addTask.Wait();

                        var productVariantImportService = DependencyResolver.Current.GetService<IProductVariantImportService>();
                        productService.ClearProductVariants(productId);
                        productVariantImportService.Import(savedItem);
                    }
                }

                Sitecore.Caching.CacheManager.ClearAllCaches();
            }
        }
    }
}
