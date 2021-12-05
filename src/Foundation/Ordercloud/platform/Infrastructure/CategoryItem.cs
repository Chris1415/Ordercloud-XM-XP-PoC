using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Infrastructure
{
    public class CategoryItem
    {
        public void OnItemSaved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;
            var itemChanges = Event.ExtractParameter(args, 1) as ItemChanges;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (savedItem == null || !"master".Equals(savedItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!savedItem.TemplateID.Equals(Products.Templates.Category.ID))
                return;

            if (savedItem.Name.Equals("__Standard Values"))
            {
                return;
            }

            using (new DatabaseSwitcher(savedItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var catalogService = DependencyResolver.Current.GetService<ICatalogService>();
                var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

                var catalog = catalogService.GetProductCatalog(savedItem);
                string catalogId = catalogService.GetCatalogId(catalog);
                string categoryId = savedItem[Products.Templates.Category.Fields.Id];
                string oldCategoryId = itemChanges.FieldChanges[Products.Templates.Category.Fields.Id]?.OriginalValue;
                string categoryParent = savedItem.Parent != catalog
                    ? categoryService.GetCategoryId(savedItem.Parent)
                    : string.Empty;
                string name = savedItem[Products.Templates.Category.Fields.Name];

                // TODO MAKE SOMEHOW GENERIC 
                using (new EventDisabler())
                {
                    savedItem.Editing.BeginEdit();
                    string displayNameChange = itemChanges.FieldChanges[Products.Templates.Base.Fields.DisplayName]?.Value;
                    if (!string.IsNullOrEmpty(displayNameChange))
                    {
                        savedItem[Products.Templates.Category.Fields.Name] = displayNameChange;
                        name = displayNameChange;
                    }

                    string nameFieldChange = itemChanges.FieldChanges[Products.Templates.Category.Fields.Name]?.Value;
                    if (!string.IsNullOrEmpty(nameFieldChange))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = nameFieldChange;
                    }

                    if (oldCategoryId != categoryId && !string.IsNullOrEmpty(oldCategoryId))
                    {
                        categoryId = oldCategoryId;
                        savedItem[Products.Templates.Category.Fields.Id] = categoryId;
                        savedItem.Name = categoryId;
                    }

                    if (savedItem.Name != categoryId && !string.IsNullOrEmpty(categoryId))
                    {
                        savedItem.Name = categoryId;
                    }
                    else if (savedItem.Name != categoryId && !string.IsNullOrEmpty(savedItem.Name))
                    {
                        savedItem[Products.Templates.Category.Fields.Id] = savedItem.Name;
                        categoryId = savedItem.Name;
                    }

                    if (savedItem.DisplayName != name && !string.IsNullOrEmpty(name))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = name;
                    }
                    else if (savedItem.DisplayName != name && !string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Category.Fields.Name] = savedItem.DisplayName;
                        name = savedItem.DisplayName;
                    }
                    else if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = savedItem.Name;
                        savedItem[Products.Templates.Category.Fields.Name] = savedItem.Name;
                        name = savedItem.Name;
                    }

                    savedItem.Editing.EndEdit();
                }

                Sitecore.Caching.CacheManager.ClearAllCaches();
                // END

                Category category = new Category()
                {
                    Active = savedItem[Products.Templates.Category.Fields.IsActive] == "1",
                    Description = savedItem[Products.Templates.Category.Fields.Description],
                    Name = name,
                    ID = categoryId,
                    ParentID = categoryParent
                };

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task<Category> task = Task.Run(() => ordercloudAsyncService.CreateOrPatchCategoryAsync(client, catalogId, categoryId, category));
                task.Wait();
            }
        }

        public void OnItemDeleting(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item deletingItem = Event.ExtractParameter(args, 0) as Item;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (deletingItem == null || !"master".Equals(deletingItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!deletingItem.TemplateID.Equals(Products.Templates.Category.ID))
                return;

            if (deletingItem.Name.Equals("__Standard Values"))
            {
                return;
            }

            using (new DatabaseSwitcher(deletingItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var catalogService = DependencyResolver.Current.GetService<ICatalogService>();
                var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
                var categoryId = categoryService.GetCategoryId(deletingItem);
                var catalog = catalogService.GetProductCatalog(deletingItem);
                string catalogId = catalogService.GetCatalogId(catalog);

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task task = Task.Run(() => ordercloudAsyncService.DeleteCategoryAsync(client, catalogId, categoryId));
                task.Wait();
            }
        }
    }
}