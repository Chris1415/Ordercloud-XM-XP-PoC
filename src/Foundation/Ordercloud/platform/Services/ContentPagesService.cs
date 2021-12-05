using Sitecore.Data;
using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class ContentPagesService : IContentPagesService
    {
        public bool Generate()
        {
            var settingsItem = Sitecore.Context.Database.GetItem(Constants.Global.SettingsItemId);
            var catalogContentRootReference = settingsItem[Templates.Settings.Fields.CatalogContentRoot];
            var catalogContentRoot = Sitecore.Context.Database.GetItem(catalogContentRootReference);
            var catalogReference = settingsItem[Templates.Settings.Fields.Catalog];
            var catalog = Sitecore.Context.Database.GetItem(catalogReference);

            IterateThroughCatalog(catalog, catalogContentRoot);

            return true;
        }

        private void IterateThroughCatalog(Item catalogItem, Item catalogContentItem)
        {
            foreach (Item catalogChild in catalogItem.Children)
            {
                var existingCatalogContentItem = catalogContentItem.Children.FirstOrDefault(element => element.Name == catalogChild.Name);
                if (existingCatalogContentItem == null)
                {
                    existingCatalogContentItem = catalogContentItem.Add(catalogChild.Name, new TemplateID(catalogChild.TemplateID == Products.Templates.Category.ID
                    ? Products.Templates.ProductCategoryPage.ID
                    : Products.Templates.ProductDetailPage.ID));
                }

                existingCatalogContentItem.Editing.BeginEdit();
                existingCatalogContentItem[Products.Templates.Base.Fields.DisplayName] = catalogChild.DisplayName;
                existingCatalogContentItem["pageTitle"] = catalogChild.DisplayName;
                if (existingCatalogContentItem.TemplateID == Products.Templates.ProductCategoryPage.ID)
                {
                    existingCatalogContentItem[Products.Templates.ProductCategoryPage.Fields.CategoryReference] = catalogChild.ID.ToString();
                }
                else
                {
                    existingCatalogContentItem[Products.Templates.ProductDetailPage.Fields.ProductReference] = catalogChild.ID.ToString();
                }
                existingCatalogContentItem.Editing.EndEdit();

                IterateThroughCatalog(catalogChild, existingCatalogContentItem);
            }
        }
    }
}