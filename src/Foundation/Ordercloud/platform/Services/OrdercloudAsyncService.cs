using BasicCompany.Foundation.Products.Ordercloud.Extensions;
using OrderCloud.SDK;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class OrdercloudAsyncService : IOrdercloudAsyncService
    {
        public async Task DeleteProductAssignmentAsync(OrderCloudClient client, string catalogId, string productId)
        {
            try
            {
                await client.Catalogs.DeleteProductAssignmentAsync(catalogId, productId);
            }
            catch (Exception) { }

        }

        public async Task DeleteProductAssignmentAsync(OrderCloudClient client, string catalogId, string categoryId, string productId)
        {
            try
            {
                await client.Categories.DeleteProductAssignmentAsync(catalogId, categoryId, productId);
            }
            catch (Exception) { }
        }

        public async Task SaveProductCategoryAssignment(OrderCloudClient client, string catalogId, CategoryProductAssignment categoryProductAssignment)
        {
            try
            {
                await client.Categories.SaveProductAssignmentAsync(catalogId, categoryProductAssignment);
            }
            catch (Exception) { }
        }

        public async Task SaveProductCatalogAssignment(OrderCloudClient client, ProductCatalogAssignment productCatalogAssignment)
        {
            try
            {
                await client.Catalogs.SaveProductAssignmentAsync(productCatalogAssignment);
            }
            catch (Exception) { }
        }
        public async Task<Product> PatchProductAsync(OrderCloudClient client, string productId, PartialProduct partialProduct)
        {
            try
            {
                // TODO Make Direction somehow editable
                var product = await client.Products.PatchAsync(productId, partialProduct);
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T> GetProductAsync<T>(OrderCloudClient client, string productId) where T : Product
        {
            try
            {
                var product = await client.Products.GetAsync<T>(productId);
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteOrderAsync(OrderCloudClient client, string orderId)
        {
            await client.Orders.DeleteAsync(OrderDirection.Outgoing, orderId);
            return true;
        }

        public async Task<IList<Order>> GetAsyncOrders(OrderCloudClient client)
        {
            // TODO Make Direction somehow editable
            var ordersRaw = await client.Orders.ListAsync(OrderDirection.Outgoing);
            return ordersRaw.Items;
        }

        public async Task<IList<LineItem>> GetLineItemsAsync(OrderCloudClient client, string orderId = null)
        {
            try
            {
                // TODO Make Direction somehow editable
                var lineItems = await client.LineItems.ListAsync(OrderDirection.Outgoing, orderId);
                return lineItems.Items;
            }
            catch (Exception)
            {
                return new List<LineItem>();
            }
        }

        public async Task<LineItem> GetLineItemAsync(OrderCloudClient client, string productId, string orderId = null)
        {
            var lineItems = await GetLineItemsAsync(client, orderId);
            return lineItems.FirstOrDefault(element => element.ProductID.Equals(productId));
        }

        public async Task<bool> DeleteLineItemAsync(OrderCloudClient client, string productId, string orderId = null)
        {
            // TODO Make Direction somehow editable
            // TODO BUG IN THE SDK -> LineItemID and OrderID are not in the right order
            await client.LineItems.DeleteAsync(OrderDirection.Outgoing, productId, orderId);
            return true;
        }

        public async Task<Order> CreateOrderAsync(OrderCloudClient client, Order order)
        {
            return await client.Orders.CreateAsync(OrderDirection.Outgoing, order);
        }

        public async Task<Category> CreateOrPatchCategoryAsync(OrderCloudClient client, string catalogId, string categoryId, Category category)
        {
            try
            {
                return await client.Categories.CreateAsync(catalogId, category);
            }
            catch (Exception e)
            {
            }

            return await client.Categories.PatchAsync(catalogId, categoryId, category.ToPartialCategory());
        }

        public async Task<Catalog> CreateOrPatchCatalogAsync(OrderCloudClient client, string catalogId, Catalog catalog)
        {
            try
            {
                return await client.Catalogs.CreateAsync(catalog);
            }
            catch (Exception e)
            {
            }

            return await client.Catalogs.PatchAsync(catalogId, catalog.ToPartialCatalog());
        }

        public async Task<Spec> CreateOrPatchSpecAsync(OrderCloudClient client, string specId, Spec spec)
        {
            try
            {
                return await client.Specs.CreateAsync(spec);
            }
            catch (Exception e)
            {
            }

            return await client.Specs.PatchAsync(specId, spec.ToPartialSpec());
        }
        public async Task<SpecOption> CreateOrPatchSpecOptionAsync(OrderCloudClient client, string specId, string specOptionId, SpecOption specOption)
        {
            try
            {
                return await client.Specs.CreateOptionAsync(specId, specOption);
            }
            catch (Exception e)
            {
            }

            return await client.Specs.PatchOptionAsync(specId, specOptionId, specOption.ToPartialSpecOption());
        }

        public async Task<LineItem> CreateOrPatchLineItem(OrderCloudClient client, OrderDirection orderDirection, string orderId, LineItem lineItem)
        {
            try
            {
                return await client.LineItems.CreateAsync(orderDirection, orderId, lineItem);
            }
            catch (Exception e)
            {
                Log.Error(e.Message, this);
            }

            try
            {
                return await client.LineItems.PatchAsync(orderDirection, orderId, lineItem.ID, lineItem.ToPartialLineItem());
            }
            catch (Exception e)
            {
                Log.Error(e.Message, this);
            }

            return null;
         
        }

        public async Task AddSpecProductAssignmentAsync(OrderCloudClient client, SpecProductAssignment specProductAssignment)
        {
            try
            {
                await client.Specs.SaveProductAssignmentAsync(specProductAssignment);
            }
            catch (Exception e)
            {
            }
        }

        public async Task<Product> GenerateProductVariants(OrderCloudClient client, string productId, bool overwrite = false)
        {
            try
            {
                return await client.Products.GenerateVariantsAsync(productId, overwrite);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task DeleteSpecProductAssignmentAsync(OrderCloudClient client, string specId, string productId)
        {
            try
            {
                await client.Specs.DeleteProductAssignmentAsync(specId, productId);
            }
            catch (Exception e)
            {
            }
        }

        public async Task DeleteCatalogAsync(OrderCloudClient client, string catalogId)
        {
            try
            {
                await client.Catalogs.DeleteAsync(catalogId);
            }
            catch (Exception e)
            {
            }
        }

        public async Task DeleteCategoryAsync(OrderCloudClient client, string catalogId, string categoryId)
        {
            try
            {
                await client.Categories.DeleteAsync(catalogId, categoryId);
            }
            catch (Exception e)
            {
            }
        }

        public async Task DeleteProductAsync(OrderCloudClient client, string productId)
        {
            try
            {
                await client.Products.DeleteAsync(productId);
            }
            catch (Exception e)
            {
            }
        }

        public async Task DeleteSpecAsync(OrderCloudClient client, string specId)
        {
            try
            {
                await client.Specs.DeleteAsync(specId);
            }
            catch (Exception e)
            {
            }
        }

        public async Task DeleteSpecOptionAsync(OrderCloudClient client, string specId, string specOptionId)
        {
            try
            {
                await client.Specs.DeleteOptionAsync(specId, specOptionId);
            }
            catch (Exception e)
            {
            }
        }

        public async Task<IList<Variant>> ListVariantsAsync(OrderCloudClient client, string productId)
        {
            try
            {
                var variants = await client.Products.ListVariantsAsync(productId);
                return variants.Items;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}