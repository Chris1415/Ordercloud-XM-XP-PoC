using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Extensions
{
    public static class SpecOptionExtensions
    {
        public static SpecOption ToSpecOption(this PartialSpecOption partialSpec)
        {
            return new SpecOption()
            {
                PriceMarkupType = partialSpec.PriceMarkupType,
                PriceMarkup = partialSpec.PriceMarkup,
                ID = partialSpec.ID,
                Value = partialSpec.Value,
            };
        }

        public static PartialSpecOption ToPartialSpecOption(this SpecOption spec)
        {
            return new PartialSpecOption()
            {
                PriceMarkupType = spec.PriceMarkupType,
                PriceMarkup = spec.PriceMarkup,
                ID = spec.ID,
                Value = spec.Value,
            };
        }
    }
}