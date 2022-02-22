using OrderCloud.SDK;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface IOrdercloudAsyncService
    {
        Task DeleteProductAssignmentAsync(OrderCloudClient client, string catalogId, string productId);
        Task DeleteProductAssignmentAsync(OrderCloudClient client, string catalogId, string categoryId, string productId);
        Task SaveProductCategoryAssignment(OrderCloudClient client, string catalogId, CategoryProductAssignment categoryProductAssignment);
        Task SaveProductCatalogAssignment(OrderCloudClient client, ProductCatalogAssignment productCatalogAssignment);
        Task<Product> PatchProductAsync(OrderCloudClient client, string productId, PartialProduct partialProduct);
        Task<T> GetProductAsync<T>(OrderCloudClient client, string productId) where T : Product;
        Task<bool> DeleteOrderAsync(OrderCloudClient client, string orderId);
        Task<IList<Order>> GetAsyncOrders(OrderCloudClient client);
        Task<LineItem> GetLineItemAsync(OrderCloudClient client, string productId, string orderId = null);
        Task<IList<LineItem>> GetLineItemsAsync(OrderCloudClient client, string orderId = null);
        Task<bool> DeleteLineItemAsync(OrderCloudClient client, string productId, string orderId = null);
        Task<Order> CreateOrderAsync(OrderCloudClient client, Order order);
        Task<Category> CreateOrPatchCategoryAsync(OrderCloudClient client, string catalogId, string categoryId, Category category);
        Task<Catalog> CreateOrPatchCatalogAsync(OrderCloudClient client, string catalogId, Catalog catalog);
        Task<Spec> CreateOrPatchSpecAsync(OrderCloudClient client, string specId, Spec spec);
        Task<SpecOption> CreateOrPatchSpecOptionAsync(OrderCloudClient client, string specId, string specOptionId, SpecOption specOption);
        Task DeleteCatalogAsync(OrderCloudClient client, string catalogId);
        Task DeleteCategoryAsync(OrderCloudClient client, string catalogId, string categoryId);
        Task DeleteProductAsync(OrderCloudClient client, string productId);
        Task DeleteSpecAsync(OrderCloudClient client, string specId);
        Task DeleteSpecOptionAsync(OrderCloudClient client, string specId, string specOptionId);
        Task AddSpecProductAssignmentAsync(OrderCloudClient client, SpecProductAssignment specProductAssignment);
        Task DeleteSpecProductAssignmentAsync(OrderCloudClient client, string specId, string productId);
        Task<Product> GenerateProductVariants(OrderCloudClient client, string productId, bool overwrite = false);
        Task<LineItem> CreateOrPatchLineItem(OrderCloudClient client, OrderDirection orderDirection, string orderId, LineItem lineItem);
        Task<IList<Variant>> ListVariantsAsync(OrderCloudClient client, string productId);
    }
}