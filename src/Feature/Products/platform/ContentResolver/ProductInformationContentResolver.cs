using System.Collections.Generic;
using System.Linq;
using BasicCompany.Feature.Products.Models;
using Sitecore.Data.Fields;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using Sitecore.Data.Items;

namespace BasicCompany.Feature.Products.ContentResolver
{
    public class ProductInformationContentResolver : RenderingContentsResolver
    {
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var contextItem = Sitecore.Context.Item;
            var productReferenceItem = ((ReferenceField)contextItem.Fields[Foundation.Products.Templates.ProductDetailPage.Fields.ProductReference])?.TargetItem;
            var productItem = ((ReferenceField)productReferenceItem.Fields[Foundation.Products.Templates.ProductReference.Fields.ProductReference])?.TargetItem;

            var imageItem = ((ImageField)productItem?.Fields[Foundation.Products.Templates.ProductCmsExtension.Fields.Image])?.MediaItem;
            string id = productItem?.Fields[Foundation.Products.Templates.ProductData.Fields.Id]?.Value ?? string.Empty;
            string name = productItem?.Fields[Foundation.Products.Templates.ProductData.Fields.Name]?.Value ?? string.Empty;
            string description = productItem.Fields[Foundation.Products.Templates.ProductData.Fields.Description]?.Value ?? string.Empty;

            MediaUrlBuilderOptions options = MediaUrlBuilderOptions.Empty;
            options.AlwaysIncludeServerUrl = true;

            return new
            {
                Id = id,
                Name = name,
                Image = imageItem != null ? MediaManager.GetMediaUrl(imageItem, options) : string.Empty,
                Description = description,
                Variants = GetProductVariants(productItem)
            };
        }
        private IList<ProductVariantModel> GetProductVariants(Item productItem)
        {
            var resultList = new List<ProductVariantModel>();
            if (productItem == null)
            {
                return resultList;
            }

            foreach (Item variant in productItem.Children)
            {
                var imageItem = ((ImageField)variant.Fields[Foundation.Products.Templates.ProductCmsExtension.Fields.Image])?.MediaItem;
                var imageItemProduct = ((ImageField)productItem.Fields[Foundation.Products.Templates.ProductCmsExtension.Fields.Image])?.MediaItem;
                resultList.Add(new ProductVariantModel()
                {
                    Id = variant[Foundation.Products.Templates.ProductData.Fields.Id],
                    Name = variant[Foundation.Products.Templates.ProductData.Fields.Name],
                    Specs = GetVariantSpecs(variant),
                    ImageUrl = imageItem != null 
                    ? MediaManager.GetMediaUrl(imageItem) 
                    : imageItemProduct != null 
                        ? MediaManager.GetMediaUrl(imageItemProduct) 
                        : string.Empty
                });
            }

            return resultList;
        }

        private IList<SpecModel> GetVariantSpecs(Item variant)
        {
            var result = new List<SpecModel>();
            foreach (Item spec in variant.Children)
            {
                var specOption = ((LookupField)spec.Fields[Foundation.Products.Templates.SpecOptionReference.Fields.SpecReference])?.TargetItem;
                result.Add(new SpecModel()
                {
                    Id = specOption[Foundation.Products.Templates.SpecOption.Fields.Id],
                    Value = specOption[Foundation.Products.Templates.SpecOption.Fields.Value]
                });
            }

            return result;
        }
    }
}