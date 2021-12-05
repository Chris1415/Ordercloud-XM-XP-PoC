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
    public class CatalogItem
    {
        public void OnItemDeleting(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item deletingItem = Event.ExtractParameter(args, 0) as Item;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (deletingItem == null || !"master".Equals(deletingItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!deletingItem.TemplateID.Equals(Products.Templates.Catalog.ID))
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
                string catalogId = catalogService.GetCatalogId(deletingItem);

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task task = Task.Run(() => ordercloudAsyncService.DeleteCatalogAsync(client, catalogId));
                task.Wait();
            }
        }

        public void OnItemSaved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;
            var itemChanges = Event.ExtractParameter(args, 1) as ItemChanges;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (savedItem == null || !"master".Equals(savedItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!savedItem.TemplateID.Equals(Products.Templates.Catalog.ID))
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

                string catalogId = catalogService.GetCatalogId(savedItem);
                string oldCatalogId = itemChanges.FieldChanges[Products.Templates.Catalog.Fields.Id]?.OriginalValue;
                string name = savedItem[Products.Templates.Catalog.Fields.Name];

                // TODO MAKE SOMEHOW GENERIC 
                using (new EventDisabler())
                {
                    savedItem.Editing.BeginEdit();
                    string displayNameChange = itemChanges.FieldChanges[Products.Templates.Base.Fields.DisplayName]?.Value;
                    if (!string.IsNullOrEmpty(displayNameChange))
                    {
                        savedItem[Products.Templates.Catalog.Fields.Name] = displayNameChange;
                        name = displayNameChange;
                    }

                    string nameFieldChange = itemChanges.FieldChanges[Products.Templates.Catalog.Fields.Name]?.Value;
                    if (!string.IsNullOrEmpty(nameFieldChange))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = nameFieldChange;
                    }

                    if (oldCatalogId != catalogId && !string.IsNullOrEmpty(oldCatalogId))
                    {
                        catalogId = oldCatalogId;
                        savedItem[Products.Templates.Catalog.Fields.Id] = catalogId;
                    }

                    if (savedItem.Name != catalogId && !string.IsNullOrEmpty(catalogId))
                    {
                        savedItem.Name = catalogId;
                    }
                    else if (savedItem.Name != catalogId && !string.IsNullOrEmpty(savedItem.Name))
                    {
                        savedItem[Products.Templates.Catalog.Fields.Id] = savedItem.Name;
                        catalogId = savedItem.Name;
                    }

                    if (savedItem.DisplayName != name && !string.IsNullOrEmpty(name))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = name;
                    }
                    else if (savedItem.DisplayName != name && !string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Catalog.Fields.Name] = savedItem.DisplayName;
                        name = savedItem.DisplayName;
                    }
                    else if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = savedItem.Name;
                        savedItem[Products.Templates.Catalog.Fields.Name] = savedItem.Name;
                        name = savedItem.Name;
                    }

                    savedItem.Editing.EndEdit();
                }

                Sitecore.Caching.CacheManager.ClearAllCaches();
                // END 


                Catalog catalog = new Catalog()
                {
                    Active = savedItem[Products.Templates.Catalog.Fields.IsActive] == "1",
                    Description = savedItem[Products.Templates.Catalog.Fields.Description],
                    Name = name,
                    ID = catalogId,
                };

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task<Catalog> task = Task.Run(() => ordercloudAsyncService.CreateOrPatchCatalogAsync(client, catalogId, catalog));
                task.Wait();
            }
        }
    }
}