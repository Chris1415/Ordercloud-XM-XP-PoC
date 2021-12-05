using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class ProductVariantSpecImportService : BaseOrdercloudService, IProductVariantSpecImportService
    {
        public ProductVariantSpecImportService(
            IOrdercloudWebclient ordercloudWebclient) : base(ordercloudWebclient)
        {
        }

        protected virtual OrderCloudClient Client => _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

        public virtual bool Import()
        {
            var client = Client;
            var specs = client.Specs.ListAsync().Result;

            foreach (var spec in specs.Items)
            {
                string specId = spec.ID;
                string displayName = spec.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                bool required = spec.Required;
                bool definesVariant = spec.DefinesVariant;

                var root = Context.Database.GetItem(Products.Constants.Global.ProductSpecRootItemId);
                if (root == null)
                {
                    return false;
                }

                var existingSpec = root.Children.FirstOrDefault(element => element.Name.Equals(specId));
                if (existingSpec == null)
                {
                    using (new EventDisabler())
                    {
                        existingSpec = root.Add(specId, new TemplateID(Products.Templates.Spec.ID));
                    }
                }

                using (new EventDisabler())
                {
                    existingSpec.Editing.BeginEdit();
                    existingSpec[Products.Templates.Spec.Fields.Id] = specId;
                    existingSpec[Products.Templates.Spec.Fields.Name] = name;
                    existingSpec[Products.Templates.Spec.Fields.Required] = required ? "1" : "0";
                    existingSpec[Products.Templates.Spec.Fields.DefinesVariant] = definesVariant ? "1" : "0";
                    existingSpec.Editing.EndEdit();
                }

                foreach (var specOption in spec.Options)
                {
                    string id = specOption.ID;
                    string specOptionName = ItemUtil.ProposeValidItemName(id);
                    string specOptionDisplayName = specOption.Value;
                    string val = specOption.Value;
                    decimal? priceMarkup = specOption.PriceMarkup;
                    var priceMarkupType = specOption.PriceMarkupType;

                    var existingSpecOption = existingSpec.Children.FirstOrDefault(element => element.Name.Equals(specOptionName));
                    if (existingSpecOption == null)
                    {
                        using (new EventDisabler())
                        {
                            existingSpecOption = existingSpec.Add(specOptionName, new TemplateID(Products.Templates.SpecOption.ID));
                        }
                    }

                    using (new EventDisabler())
                    {
                        existingSpecOption.Editing.BeginEdit();
                        existingSpecOption[Products.Templates.SpecOption.Fields.Id] = id;
                        existingSpecOption[Products.Templates.Base.Fields.DisplayName] = specOptionDisplayName;
                        existingSpecOption[Products.Templates.SpecOption.Fields.Value] = val;
                        existingSpecOption[Products.Templates.SpecOption.Fields.PriceMarkup] = priceMarkup.HasValue ? priceMarkup.Value.ToString() : "0.0";
                        existingSpecOption[Products.Templates.SpecOption.Fields.PriceMarkupType] = priceMarkup.ToString();
                        existingSpecOption.Editing.EndEdit();
                    }
                }
            }

            Sitecore.Caching.CacheManager.ClearAllCaches();
            return true;
        }
    }
}
