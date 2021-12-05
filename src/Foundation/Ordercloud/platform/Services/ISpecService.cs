using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface ISpecService
    {
        Item GetSpecOption(string specId, string specOptionId);
        Item GetSpec(string specId);
    }
}
