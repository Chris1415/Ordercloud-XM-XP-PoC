using BasicCompany.Foundation.Products.Ordercloud.Models;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using BasicCompany.Foundation.Products;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Linq;
using OrderCloud.SDK;
using BasicCompany.Foundation.Products.Ordercloud.Services.Base;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class SupplierImportService : BaseOrdercloudService, ISupplierImportService
    {
        public SupplierImportService(IOrdercloudWebclient ordercloudWebclient) : base(ordercloudWebclient)
        {
        }

        public bool Import()
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            var suppliers = client.Suppliers.ListAsync().Result;

            var root = Sitecore.Context.Database.GetItem(Products.Constants.Global.SuppliersRootItemId);
            if (root == null)
            {
                return false;
            }

            foreach (var supplier in suppliers.Items)
            {
                string displayName = supplier.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string id = supplier.ID;
                bool isActive = supplier.Active;

                var addresses = client.SupplierAddresses.ListAsync(id).Result;

                // Take first for now ^^
                var firstAddress = addresses.Items.FirstOrDefault();
                var address = new AddressModel()
                {
                    City = firstAddress?.City ?? string.Empty,
                    Country = firstAddress?.Country ?? string.Empty,
                    Name = firstAddress?.AddressName ?? string.Empty,
                    Phone = firstAddress?.Phone ?? string.Empty,
                    Street = firstAddress?.Street1 ?? string.Empty,
                    Street2 = firstAddress?.Street2 ?? string.Empty,
                    Zip = firstAddress?.Zip ?? string.Empty,
                    State = firstAddress?.State ?? string.Empty,
                    CompanyName = firstAddress?.CompanyName ?? string.Empty,
                    FirstName = firstAddress?.FirstName ?? string.Empty,
                    LastName = firstAddress?.LastName ?? string.Empty,
                };


                var existingItem = root.Children.FirstOrDefault(element => element.Name.Equals(name));
                if (existingItem == null)
                {
                    existingItem = root.Add(name, new TemplateID(Products.Templates.SupplierBranch.ID));
                }

                existingItem.Editing.BeginEdit();
                existingItem[Products.Templates.Supplier.Fields.Name] = name;
                existingItem[Products.Templates.Supplier.Fields.Id] = id;
                existingItem[Products.Templates.Supplier.Fields.IsActive] = isActive ? "1" : "0";

                existingItem[Products.Templates.Address.Fields.AddressName] = address.Name;
                existingItem[Products.Templates.Address.Fields.City] = address.City;
                existingItem[Products.Templates.Address.Fields.State] = address.State;
                existingItem[Products.Templates.Address.Fields.Country] = address.Country;
                existingItem[Products.Templates.Address.Fields.LastName] = address.LastName;
                existingItem[Products.Templates.Address.Fields.FirstName] = address.FirstName;
                existingItem[Products.Templates.Address.Fields.Phone] = address.Phone;
                existingItem[Products.Templates.Address.Fields.Street] = address.Street;
                existingItem[Products.Templates.Address.Fields.Zip] = address.Zip;
                existingItem[Products.Templates.Address.Fields.CompanyName] = address.CompanyName;
                existingItem[Products.Templates.Address.Fields.AddressName] = address.Name;
                existingItem.Editing.EndEdit();
            }

            return true;
        }
    }
}