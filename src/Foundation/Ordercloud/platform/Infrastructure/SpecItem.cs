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
    public class SpecItem
    {
        public void OnItemDeleting(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item deletingItem = Event.ExtractParameter(args, 0) as Item;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (deletingItem == null || !"master".Equals(deletingItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!deletingItem.TemplateID.Equals(Products.Templates.Spec.ID))
                return;

            if (deletingItem.Name.Equals("__Standard Values"))
            {
                return;
            }

            using (new DatabaseSwitcher(deletingItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();;
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var specId = deletingItem[Products.Templates.Spec.Fields.Id];

                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();
                Task innerTask = Task.Run(() => ordercloudAsyncService.DeleteProductAsync(client, specId));
                innerTask.Wait();
            }

            // TODO UPDATE PRODCUTS NOW
        }

        public void OnItemSaving(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Event.ExtractParameter(args, 0) as Item;
            var itemChanges = Event.ExtractParameter(args, 1) as ItemChanges;

            // If we don't have an item or we're not saving in the master DB, ignore this save
            if (savedItem == null || !"master".Equals(savedItem.Database?.Name, StringComparison.OrdinalIgnoreCase))
                return;

            if (!savedItem.TemplateID.Equals(Products.Templates.Spec.ID))
                return;

            using (new DatabaseSwitcher(savedItem.Database))
            {
                var ordercloudWebclient = DependencyResolver.Current.GetService<IOrdercloudWebclient>();
                var client = ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
                var ordercloudAsyncService = DependencyResolver.Current.GetService<IOrdercloudAsyncService>();

                string specId = savedItem[Products.Templates.Spec.Fields.Id];
                string oldSpecId = itemChanges.FieldChanges[Products.Templates.Spec.Fields.Id]?.OriginalValue;
                string name = savedItem[Products.Templates.Spec.Fields.Name];

                using (new EventDisabler())
                {
                    savedItem.Editing.BeginEdit();
                    string displayNameChange = itemChanges.FieldChanges[Products.Templates.Base.Fields.DisplayName]?.Value;
                    if (!string.IsNullOrEmpty(displayNameChange))
                    {
                        savedItem[Products.Templates.Spec.Fields.Name] = displayNameChange;
                        name = displayNameChange;
                    }

                    string nameFieldChange = itemChanges.FieldChanges[Products.Templates.Category.Fields.Name]?.Value;
                    if (!string.IsNullOrEmpty(nameFieldChange))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = nameFieldChange;
                    }

                    if (oldSpecId != specId && !string.IsNullOrEmpty(oldSpecId))
                    {
                        specId = oldSpecId;
                        savedItem[Products.Templates.Spec.Fields.Id] = specId;
                        savedItem.Name = specId;
                    }

                    if (savedItem.Name != specId && !string.IsNullOrEmpty(specId))
                    {
                        savedItem.Name = specId;
                    }
                    else if (savedItem.Name != specId && !string.IsNullOrEmpty(savedItem.Name))
                    {
                        savedItem[Products.Templates.Spec.Fields.Id] = savedItem.Name;
                        specId = savedItem.Name;
                    }

                    if (savedItem.DisplayName != name && !string.IsNullOrEmpty(name))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = name;
                    }
                    else if (savedItem.DisplayName != name && !string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Spec.Fields.Name] = savedItem.DisplayName;
                        name = savedItem.DisplayName;
                    }
                    else if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(savedItem.DisplayName))
                    {
                        savedItem[Products.Templates.Base.Fields.DisplayName] = savedItem.Name;
                        savedItem[Products.Templates.Spec.Fields.Name] = savedItem.Name;
                        name = savedItem.Name;
                    }

                    savedItem.Editing.EndEdit();
                }

                Sitecore.Caching.CacheManager.ClearAllCaches();

                Spec spec = new Spec()
                {
                    ID = specId,
                    Name = name,
                    Required = ((CheckboxField)savedItem.Fields[Products.Templates.Spec.Fields.Required])?.Checked ?? false,
                    DefinesVariant = ((CheckboxField)savedItem.Fields[Products.Templates.Spec.Fields.DefinesVariant])?.Checked ?? false,
                };

                Task<Spec> task = Task.Run(() => ordercloudAsyncService.CreateOrPatchSpecAsync(client, specId, spec));
                task.Wait();              
            }
        }
    }
}