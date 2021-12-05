using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Infrastructure
{
    public class ProductReferenceItem
    {
        public void OnItemDeleting(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item deletingItem = Event.ExtractParameter(args, 0) as Item;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (deletingItem == null || !"master".Equals(deletingItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!deletingItem.TemplateID.Equals(Products.Templates.ProductReference.ID))
                return;

            if (deletingItem.Name.Equals("__Standard Values"))
            {
                return;
            }

            using (new DatabaseSwitcher(deletingItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var catalogService = DependencyResolver.Current.GetService<ICatalogService>();
                var productReferenceService = DependencyResolver.Current.GetService<IProductReferenceService>();
                var productService = DependencyResolver.Current.GetService<IProductService>();
                var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var product = productReferenceService.GetProduct(deletingItem);
                var productId = productService.GetProductId(product);
                var category = deletingItem.Parent;
                var categoryId = categoryService.GetCategoryId(category);
                var catalog = catalogService.GetProductCatalog(deletingItem);              
                string catalogId = catalogService.GetCatalogId(catalog);

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task innerTask = Task.Run(() => ordercloudAsyncService.DeleteProductAssignmentAsync(client, catalogId, productId));
                innerTask.Wait();

                // Delte Old Product Category Reference
                Task task = Task.Run(() => ordercloudAsyncService.DeleteProductAssignmentAsync(client, catalogId, categoryId, productId));
                task.Wait();
            }
        }

        public void OnItemMoved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;
            ID oldParent = Event.ExtractParameter(args, 1) as ID;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (savedItem == null || !"master".Equals(savedItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!savedItem.TemplateID.Equals(Products.Templates.ProductReference.ID))
                return;

            using (new DatabaseSwitcher(savedItem.Database))
            {
                var catalogService = DependencyResolver.Current.GetService<ICatalogService>();
                var oldParentItem = Sitecore.Context.Database.GetItem(oldParent);
                string oldParentItemId = oldParentItem.Name;
                var oldCatalogItem = catalogService.GetProductCatalog(oldParentItem);
                string oldCatalogItemId = oldCatalogItem?.Name ?? string.Empty;

                if (oldCatalogItem == null)
                {
                    return;
                }

                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var productService = DependencyResolver.Current.GetService<IProductService>();
                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                var productReferenceService = DependencyResolver.Current.GetService<IProductReferenceService>();

                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

                // Update Product Reference
                var product = productReferenceService.GetProduct(savedItem);
                string productId = productService.GetProductId(product);
                var catalog = catalogService.GetProductCatalog(savedItem);
                string catalogId = catalog?.Name ?? string.Empty;

                if (catalog == null)
                {
                    return;
                }

                // Delete Old Product Catalog Reference
                if (oldCatalogItemId != catalogId)
                {
                    Task innerTask = Task.Run(() => ordercloudAsyncService.DeleteProductAssignmentAsync(client, oldCatalogItemId, productId));
                    innerTask.Wait();
                }

                // Delte Old Product Category Reference
                Task task = Task.Run(() => ordercloudAsyncService.DeleteProductAssignmentAsync(client, catalogId, oldParentItemId, productId));
                task.Wait();

                ProductCatalogAssignment productCatalogAssignment = new ProductCatalogAssignment()
                {
                    ProductID = productId,
                    CatalogID = catalogId,
                };

                task = Task.Run(() => ordercloudAsyncService.SaveProductCatalogAssignment(client, productCatalogAssignment));
                task.Wait();

                // Update Category Reference
                var category = savedItem.Parent;
                string categoryId = category.Name;
                CategoryProductAssignment categoryProductAssignment = new CategoryProductAssignment()
                {
                    ProductID = productId,
                    CategoryID = categoryId,
                };

                task = Task.Run(() => ordercloudAsyncService.SaveProductCategoryAssignment(client, catalogId, categoryProductAssignment));
                task.Wait();
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

            if (!savedItem.TemplateID.Equals(Products.Templates.ProductReference.ID))
                return;

            using (new DatabaseSwitcher(savedItem.Database))
            {
                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                var productService = DependencyResolver.Current.GetService<IProductService>();
                var catalogService = DependencyResolver.Current.GetService<ICatalogService>();
                var productReferenceService = DependencyResolver.Current.GetService<IProductReferenceService>();
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var categoryService = DependencyResolver.Current.GetService<ICategoryService>();

                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

                var product = productReferenceService.GetProduct(savedItem);
                string productId = productService.GetProductId(product);
                var catalog = catalogService.GetProductCatalog(savedItem);
                string catalogId = catalog?.Name ?? string.Empty;
                var category = savedItem.Parent;
                string categoryId = categoryService.GetCategoryId(category);

                if (catalog == null)
                {
                    return;
                }

                var oldProductReference = itemChanges.FieldChanges[Products.Templates.ProductReference.Fields.ProductReference]?.OriginalValue ?? string.Empty;
                if (!string.IsNullOrEmpty(oldProductReference))
                {
                    var oldProduct = Sitecore.Context.Database.GetItem(oldProductReference);
                    string oldProductId = productService.GetProductId(oldProduct);
                    // Execute Delete Assignments
                    // Delete Old Product Catalog Reference
                    Task innerTask = Task.Run(() => ordercloudAsyncService.DeleteProductAssignmentAsync(client, catalogId, oldProductId));
                    innerTask.Wait();

                    // Delte Old Product Category Reference
                    Task task = Task.Run(() => ordercloudAsyncService.DeleteProductAssignmentAsync(client, catalogId, categoryId, oldProductId));
                    task.Wait();
                }

                if (!string.IsNullOrEmpty(productId))
                {
                    // Execute Create Assignments
                    ProductCatalogAssignment productCatalogAssignment = new ProductCatalogAssignment()
                    {
                        ProductID = productId,
                        CatalogID = catalogId,
                    };

                    Task task = Task.Run(() => ordercloudAsyncService.SaveProductCatalogAssignment(client, productCatalogAssignment));
                    task.Wait();

                    CategoryProductAssignment categoryProductAssignment = new CategoryProductAssignment()
                    {
                        ProductID = productId,
                        CategoryID = categoryId,
                    };

                    task = Task.Run(() => ordercloudAsyncService.SaveProductCategoryAssignment(client, catalogId, categoryProductAssignment));
                    task.Wait();

                    // Rename current item to get the name of the linked product
                    using (new EventDisabler())
                    {
                        savedItem.Editing.BeginEdit();
                        savedItem.Name = productId;
                        savedItem.Editing.EndEdit();
                    }
                }
            }
        }
    }
}