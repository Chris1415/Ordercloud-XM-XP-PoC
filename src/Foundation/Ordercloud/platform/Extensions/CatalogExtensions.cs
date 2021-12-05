using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Extensions
{
    public static class CatalogExtensions
    {
        public static Catalog ToCatalog(this PartialCatalog partialCatalog)
        {
            return new Catalog()
            {
                Active = partialCatalog.Active,
                Description = partialCatalog.Description,
                ID = partialCatalog.ID,
                Name = partialCatalog.Name,
            };
        }

        public static PartialCatalog ToPartialCatalog(this Catalog category)
        {
            return new PartialCatalog()
            {
                Active = category.Active,
                Description = category.Description,
                ID = category.ID,
                Name = category.Name,
            };
        }
    }
}