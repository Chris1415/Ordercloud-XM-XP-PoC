using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class BuyersImportService : BaseOrdercloudService, IBuyersImportService
    {
        private readonly IBuyerService _buyerService;

        public BuyersImportService(
            IOrdercloudWebclient ordercloudWebclient,
            IBuyerService buyerService) : base(ordercloudWebclient)
        {
            _buyerService = buyerService;
        }

        public bool Import()
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

            // Buyer Import
            return ImportBuyer(client);
        }

        private bool ImportBuyer(OrderCloudClient client)
        {
            var root = Context.Database.GetItem(Products.Constants.Global.BuyersRootItemId);
            if (root == null)
            {
                return false;
            }

            var buyers = client.Buyers.ListAsync().Result;
            foreach (var buyer in buyers.Items)
            {
                string displayName = buyer.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string id = buyer.ID;
                bool isActive = buyer.Active;

                var existingItem = root.Children.FirstOrDefault(element => element.Name.Equals(name));
                if (existingItem == null)
                {
                    existingItem = root.Add(name, new TemplateID(Products.Templates.BuyerBranch.ID));
                }

                existingItem.Editing.BeginEdit();
                existingItem[Products.Templates.Buyer.Fields.Name] = name;
                existingItem[Products.Templates.Buyer.Fields.Id] = id;
                existingItem[Products.Templates.Buyer.Fields.IsActive] = isActive ? "1" : "0";
                existingItem.Editing.EndEdit();

                // Buyer Address Import
                bool buyerAddressImportOk = ImportBuyerAddresses(client, existingItem, id);

                // Buyer User Import
                bool buyerUserImportOk = ImportBuyerUser(client, existingItem, id);

                // Buyer Group Import
                bool buyerUserGroupImportOk = ImportBuyerUserGroup(client, existingItem, id);
            }

            return true;
        }

        private bool ImportBuyerUserGroup(OrderCloudClient client, Item buyer, string id)
        {
            var buyerUserGroups = client.UserGroups.ListAsync(id).Result;
            foreach (var buyerUserGroup in buyerUserGroups.Items)
            {
                string buyerUserGroupId = buyerUserGroup.ID;
                string name = buyerUserGroup.Name;
                string description = buyerUserGroup.Description;
                var users = GetUserGroupAssingments(client, id, buyerUserGroupId);

                var userGroupRoot = _buyerService.UserGroupsRoot(buyer);
                var existingUser = userGroupRoot.Children.FirstOrDefault(element => element.Name.Equals(buyerUserGroupId));
                if (existingUser == null)
                {
                    existingUser = userGroupRoot.Add(buyerUserGroupId, new TemplateID(Products.Templates.BuyerUserGroup.ID));
                }

                existingUser.Editing.BeginEdit();
                existingUser[Products.Templates.BuyerUserGroup.Fields.Id] = buyerUserGroupId;
                existingUser[Products.Templates.BuyerUserGroup.Fields.Name] = name;
                existingUser[Products.Templates.BuyerUserGroup.Fields.Description] = description;
                existingUser[Products.Templates.BuyerUserGroup.Fields.Users] = string.Join("|", users.Select(element => element.ID.ToString()));
                existingUser.Editing.EndEdit();
            }

            return true;
        }

        private IList<Item> GetUserGroupAssingments(OrderCloudClient client, string id, string userGroupId)
        {
            var result = new List<Item>();
            var userGroupAssignments = client.UserGroups.ListUserAssignmentsAsync(id, userGroupId).Result;
            foreach (var userGroupAssignment in userGroupAssignments.Items)
            {
                string userId = userGroupAssignment.UserID;
                var userItem = _buyerService.BuyerUser(id, userId);
                if (userItem != null)
                {
                    result.Add(userItem);
                }
            }

            return result;
        }

        private bool ImportBuyerAddresses(OrderCloudClient client, Item buyer, string id)
        {
            var buyerAddresses = client.Addresses.ListAsync(id).Result;
            foreach (var buyerAddress in buyerAddresses.Items)
            {
                string buyerAddressId = buyerAddress.ID;
                string addressName = buyerAddress.AddressName;
                string city = buyerAddress.City;
                string companyName = buyerAddress.CompanyName;
                string country = buyerAddress.Country;
                string firstName = buyerAddress.FirstName;
                string lastName = buyerAddress.LastName;
                string phone = buyerAddress.Phone;
                string state = buyerAddress.State;
                string street1 = buyerAddress.Street1;
                string street2 = buyerAddress.Street2;
                string zip = buyerAddress.Zip;

                var addressRoot = _buyerService.AddressesRoot(buyer);
                var existingUser = addressRoot.Children.FirstOrDefault(element => element.Name.Equals(buyerAddressId));
                if (existingUser == null)
                {
                    existingUser = addressRoot.Add(buyerAddressId, new TemplateID(Products.Templates.Address.ID));
                }

                existingUser.Editing.BeginEdit();
                existingUser[Products.Templates.Address.Fields.Id] = buyerAddressId;
                existingUser[Products.Templates.Address.Fields.AddressName] = addressName;
                existingUser[Products.Templates.Address.Fields.City] = city;
                existingUser[Products.Templates.Address.Fields.CompanyName] = companyName;
                existingUser[Products.Templates.Address.Fields.Country] = country;
                existingUser[Products.Templates.Address.Fields.FirstName] = firstName;
                existingUser[Products.Templates.Address.Fields.LastName] = lastName;
                existingUser[Products.Templates.Address.Fields.Phone] = phone;
                existingUser[Products.Templates.Address.Fields.State] = state;
                existingUser[Products.Templates.Address.Fields.Street] = street1;
                existingUser[Products.Templates.Address.Fields.Street2] = street2;
                existingUser[Products.Templates.Address.Fields.Zip] = zip;
                existingUser.Editing.EndEdit();
            }

            return true;
        }

        private bool ImportBuyerUser(OrderCloudClient client, Item buyer, string id)
        {
            var buyerUsers = client.Users.ListAsync(id).Result;
            foreach (var buyerUser in buyerUsers.Items)
            {
                string buyerUserId = buyerUser.ID;
                string buyerUserName = buyerUser.Username;
                string buyerFirstName = buyerUser.FirstName;
                string buyerLastName = buyerUser.LastName;
                string buyerEmail = buyerUser.Email;
                string buyerPhone = buyerUser.Phone;
                DateTimeOffset? buyerTermsAccepted = buyerUser.TermsAccepted;
                bool buyerIsActive = buyerUser.Active;

                var usersRoot = _buyerService.UsersRoot(buyer);
                var existingUser = usersRoot.Children.FirstOrDefault(element => element.Name.Equals(buyerUserId));
                if (existingUser == null)
                {
                    existingUser = usersRoot.Add(buyerUserId, new TemplateID(Products.Templates.BuyerUser.ID));
                }

                existingUser.Editing.BeginEdit();
                existingUser[Products.Templates.User.Fields.Username] = buyerUserName;
                existingUser[Products.Templates.User.Fields.Id] = buyerUserId;
                existingUser[Products.Templates.User.Fields.IsActive] = buyerIsActive ? "1" : "0";
                existingUser[Products.Templates.User.Fields.LastName] = buyerLastName;
                existingUser[Products.Templates.User.Fields.FirstName] = buyerFirstName;
                existingUser[Products.Templates.User.Fields.Email] = buyerEmail;
                existingUser[Products.Templates.User.Fields.Phone] = buyerPhone;
                if (buyerTermsAccepted.HasValue)
                {
                    existingUser[Products.Templates.User.Fields.TermsAccepted] = DateUtil.ToIsoDate(buyerTermsAccepted.Value.DateTime);
                }
                existingUser.Editing.EndEdit();

                bool addressAssignmentsImportOk = ImportAddressAssignments(client, buyer, id, buyerUserId);
            }

            return true;
        }

        private bool ImportAddressAssignments(OrderCloudClient client, Item buyer, string buyerId, string userId, string userGroupId = null, bool useUserId = true)
        {
            var addressesToMap = new List<Item>();
            ListPage<AddressAssignment> userAddressAssignments;
            if (useUserId)
            {
                userAddressAssignments = client.Addresses.ListAssignmentsAsync(buyerId, userID: userId).Result;
            }
            else
            {
                userAddressAssignments = client.Addresses.ListAssignmentsAsync(buyerId, userGroupID: userGroupId).Result;
            }

            foreach (var userAddressAssignment in userAddressAssignments.Items)
            {
                string addressId = userAddressAssignment.AddressID;
                string addressAssignmentId = $"{addressId}-{(useUserId ? userId : userGroupId)}";
                bool isShipping = userAddressAssignment.IsShipping;
                bool isBilling = userAddressAssignment.IsBilling;
                Item addressItem = _buyerService.Address(buyerId, addressId);
                Item buyerUserItem = _buyerService.BuyerUser(buyerId, userId);
                string buyerUserGroupItemId = string.Empty;

                var usersRoot = _buyerService.AddressAssignmentsRoot(buyer);
                var existingUser = usersRoot.Children.FirstOrDefault(element => element.Name.Equals(addressAssignmentId));
                if (existingUser == null)
                {
                    existingUser = usersRoot.Add(addressAssignmentId, new TemplateID(Products.Templates.AddressAssignment.ID));
                }

                existingUser.Editing.BeginEdit();
                existingUser[Products.Templates.AddressAssignment.Fields.Address] = addressItem?.ID?.ToString() ?? string.Empty;
                existingUser[Products.Templates.AddressAssignment.Fields.BuyerUser] = buyerUserItem?.ID?.ToString() ?? string.Empty;
                existingUser[Products.Templates.AddressAssignment.Fields.BuyerUserGroup] = buyerUserGroupItemId;
                existingUser[Products.Templates.AddressAssignment.Fields.IsBilling] = isBilling ? "1" : "0";
                existingUser[Products.Templates.AddressAssignment.Fields.IsShipping] = isShipping ? "1" : "0";
                existingUser.Editing.EndEdit();
            }

            return true;
        }
    }
}

