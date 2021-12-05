using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface ICategoryService
    {
        Item GetCategoryItem(Item catalogItem, string id);

        string GetCategoryId(Item categoryItem);
    }
}
