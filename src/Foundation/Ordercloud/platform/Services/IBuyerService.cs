using Sitecore.Data.Items;
using System.Collections.Generic;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface IBuyerService
    {
        Item Buyer(string buyerId);
        Item AddressesRoot(string buyerId);
        Item UserGroupsRoot(string buyerId);
        Item UsersRoot(string buyerId);
        Item AddressesRoot(Item buyer);
        Item UserGroupsRoot(Item buyer);
        Item UsersRoot(Item buyer);
        Item AddressAssignmentsRoot(string buyerId);
        Item AddressAssignmentsRoot(Item buyer);

        Item Address(string buyerId, string addressId);
        Item BuyerUser(string buyerId, string buyerUserId);
    }
}
