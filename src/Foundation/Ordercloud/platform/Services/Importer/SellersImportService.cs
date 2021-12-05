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
    public class SellersImportService : BaseOrdercloudService, ISellersImportService
    {
        public SellersImportService(
            IOrdercloudWebclient ordercloudWebclient) : base(ordercloudWebclient)
        {
        }

        public bool Import()
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });

            // Buyer Import
            return ImportSeller(client);
        }

        private bool ImportSeller(OrderCloudClient client)
        {
            var root = Context.Database.GetItem(Products.Constants.Global.SellerRootItemId);
            if (root == null)
            {
                return false;
            }

            var sellers = client.AdminUsers.ListAsync().Result;
            foreach (var seller in sellers.Items)
            {
                string displayName = seller.Username;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string id = seller.ID;
                bool isActive = seller.Active;
                string sellerUserName = seller.Username;
                string sellerFirstName = seller.FirstName;
                string sellerLastName = seller.LastName;
                string sellerPw = seller?.Password ?? string.Empty;
                string sellerMail = seller.Email;
                string sellerPhone = seller.Phone;
                var termsAccepted = seller.TermsAccepted;


                var existingItem = root.Children.FirstOrDefault(element => element.Name.Equals(name));
                if (existingItem == null)
                {
                    existingItem = root.Add(name, new TemplateID(Products.Templates.Seller.ID));
                }

                existingItem.Editing.BeginEdit();
                existingItem[Products.Templates.User.Fields.Id] = id;
                existingItem[Products.Templates.User.Fields.IsActive] = isActive ? "1" : "0";
                existingItem[Products.Templates.User.Fields.Username] = sellerUserName;
                existingItem[Products.Templates.User.Fields.LastName] = sellerLastName;
                existingItem[Products.Templates.User.Fields.FirstName] = sellerFirstName;
                existingItem[Products.Templates.User.Fields.Email] = sellerMail;
                existingItem[Products.Templates.User.Fields.Phone] = sellerPhone;
                existingItem[Products.Templates.User.Fields.Password] = sellerPw ?? string.Empty;
                existingItem[Products.Templates.User.Fields.TermsAccepted] = termsAccepted.HasValue ? DateUtil.ToIsoDate(termsAccepted.Value.DateTime) : string.Empty;
                existingItem.Editing.EndEdit();
            }

            return true;
        }
    }
}

