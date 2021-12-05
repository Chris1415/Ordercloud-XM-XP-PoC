using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class ProductVariantImportService : BaseOrdercloudService, IProductVariantImportService
    {
        private readonly ISpecService _specService;
        private readonly IOrdercloudAsyncService _ordercloudAsyncService;

        public ProductVariantImportService(
            IOrdercloudWebclient ordercloudWebclient,
            ISpecService specService,
            IOrdercloudAsyncService ordercloudAsyncService) : base(ordercloudWebclient)
        {
            _specService = specService;
            _ordercloudAsyncService = ordercloudAsyncService;
        }

        protected virtual OrderCloudClient Client => _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

        public virtual bool Import(Item product)
        {
            var client = Client;
            // TODO REMOVE HARDCODED 
            Task<IList<Variant>> task = Task.Run(() => _ordercloudAsyncService.ListVariantsAsync(client, product.Name));
            task.Wait();
            var variants = task.Result;

            foreach (var variant in variants)
            {
                string displayName = variant.Name;
                string name = string.IsNullOrEmpty(displayName) ? string.Empty : ItemUtil.ProposeValidItemName(displayName);
                string id = variant.ID;
                string description = variant.Description;
                bool isActive = variant.Active;
                decimal? height = variant.ShipHeight;
                decimal? weight = variant.ShipWeight;
                decimal? width = variant.ShipWidth;
                decimal? length = variant.ShipLength;

                var existingItem = product.Children.FirstOrDefault(element => element.Name.Equals(id));
                if (existingItem == null)
                {
                    existingItem = product.Add(id, new TemplateID(Products.Templates.ProductVariant.ID));
                }

                existingItem.Editing.BeginEdit();
                existingItem[Products.Templates.ProductData.Fields.Name] = name ?? id;
                existingItem[Products.Templates.Base.Fields.DisplayName] = displayName;
                existingItem[Products.Templates.ProductData.Fields.Id] = id;
                existingItem[Products.Templates.ProductData.Fields.Description] = description;
                existingItem[Products.Templates.ProductData.Fields.IsActive] = isActive ? "1" : "0";
                existingItem[Products.Templates.ProductData.Fields.Height] = height.HasValue ? height.Value.ToString() : string.Empty;
                existingItem[Products.Templates.ProductData.Fields.Width] = width.HasValue ? width.Value.ToString() : string.Empty;
                existingItem[Products.Templates.ProductData.Fields.Length] = length.HasValue ? length.Value.ToString() : string.Empty;
                existingItem[Products.Templates.ProductData.Fields.Weight] = weight.HasValue ? weight.Value.ToString() : string.Empty;
                existingItem.Editing.EndEdit();

                ImportSpecs(existingItem, variant.Specs);
            }

            return true;
        }

        private bool ImportSpecs(Item variant, IReadOnlyList<VariantSpec> specs)
        {
            foreach (var spec in specs)
            {
                string specId = spec.SpecID;
                string displayName = spec.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string optionId = spec.OptionID;
                string val = spec.Value;
                decimal? priceMarkup = spec.PriceMarkup;
                var priceMarkupType = spec.PriceMarkupType;
                string specReferenceItemName = $"{specId}-{optionId}";
                Item specReference = _specService.GetSpecOption(specId, optionId);

                if (specReference == null)
                {
                    continue;
                }

                var existingSpec = variant.Children.FirstOrDefault(element => element.Name.Equals(specReferenceItemName));
                if (existingSpec == null)
                {
                    existingSpec = variant.Add(specReferenceItemName, new TemplateID(Products.Templates.SpecOptionReference.ID));
                }

                existingSpec.Editing.BeginEdit();
                existingSpec[Products.Templates.SpecOptionReference.Fields.SpecReference] = specReference.ID.ToString();
                existingSpec[Products.Templates.Base.Fields.DisplayName] = val;
                existingSpec.Editing.EndEdit();
            }

            return true;
        }
    }
}
