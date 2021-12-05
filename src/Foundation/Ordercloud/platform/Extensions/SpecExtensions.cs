using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Extensions
{
    public static class SpecExtensions
    {
        public static Spec ToSpec(this PartialSpec partialSpec)
        {
            return new Spec()
            {
                DefinesVariant = partialSpec.DefinesVariant,
                Required = partialSpec.Required,
                ID = partialSpec.ID,
                Name = partialSpec.Name,
            };
        }

        public static PartialSpec ToPartialSpec(this Spec spec)
        {
            return new PartialSpec()
            {
                Required = spec.Required,
                DefinesVariant = spec.DefinesVariant,
                ID = spec.ID,
                Name = spec.Name,
            };
        }
    }
}