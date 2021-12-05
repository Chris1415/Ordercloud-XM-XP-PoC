using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class CategoryService : ICategoryService
    {
        public Item GetCategoryItem(Item catalogItem, string categoryId)
        {
            return catalogItem.Axes.GetDescendants().FirstOrDefault(element => element.Name == categoryId);
        }

        public string GetCategoryId(Item categoryItem)
        {
            return categoryItem[Products.Templates.Category.Fields.Id];
        }
    }
}
