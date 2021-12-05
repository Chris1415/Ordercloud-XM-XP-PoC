using Sitecore.Data.Items;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class BuyerService : IBuyerService
    {
        public Item AddressesRoot(string buyerId)
        {
            var buyer = Buyer(buyerId);
            return AddressesRoot(buyer);
        }

        public Item AddressesRoot(Item buyer)
        {
            return buyer.Children.FirstOrDefault(element => element.TemplateID.Equals(Products.Templates.AddressFolder.ID));
        }

        public Item UserGroupsRoot(string buyerId)
        {
            var buyer = Buyer(buyerId);
            return UserGroupsRoot(buyer);
        }

        public Item UserGroupsRoot(Item buyer)
        {
            return buyer.Children.FirstOrDefault(element => element.TemplateID.Equals(Products.Templates.BuyerUserGroupFolder.ID));
        }

        public Item UsersRoot(string buyerId)
        {
            var buyer = Buyer(buyerId);
            return UsersRoot(buyer);
        }

        public Item UsersRoot(Item buyer)
        {
            return buyer.Children.FirstOrDefault(element => element.TemplateID.Equals(Products.Templates.BuyerUserFolder.ID));
        }

        public Item AddressAssignmentsRoot(string buyerId)
        {
            var buyer = Buyer(buyerId);
            return AddressAssignmentsRoot(buyer);
        }

        public Item AddressAssignmentsRoot(Item buyer)
        {
            return buyer.Children.FirstOrDefault(element => element.TemplateID.Equals(Products.Templates.AddressAssignmentsFolder.ID));
        }

        public Item Buyer(string buyerId)
        {
            Item buyers = Sitecore.Context.Database.GetItem(Constants.Global.Buyers);
            return buyers.Children.FirstOrDefault(element => element[Products.Templates.Buyer.Fields.Id].Equals(buyerId));
        }

        public Item Address(string buyerId, string addressId)
        {
            var addressRoot = AddressesRoot(buyerId);
            return addressRoot.Children.FirstOrDefault(element => element.Name.Equals(addressId));
        }

        public Item BuyerUser(string buyerId, string buyerUserId)
        {
            var buyerRoot = UsersRoot(buyerId);
            return buyerRoot.Children.FirstOrDefault(element => element.Name.Equals(buyerUserId));
        }
    }
}
