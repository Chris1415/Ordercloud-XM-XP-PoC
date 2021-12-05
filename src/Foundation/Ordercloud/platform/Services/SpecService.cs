using Sitecore;
using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class SpecService : ISpecService
    {
        public Item GetSpecOption(string specId, string specOptionId)
        {
            var specItem = GetSpec(specId);
            return specItem?.Children?.FirstOrDefault(element => element[Products.Templates.SpecOption.Fields.Id] == specOptionId);
        }

        public Item GetSpec(string specId)
        {
            var root = Context.Database.GetItem(Products.Constants.Global.ProductSpecRootItemId);
            return root.Children.FirstOrDefault(element => element[Products.Templates.Spec.Fields.Id] == specId);
        }
    }
}
