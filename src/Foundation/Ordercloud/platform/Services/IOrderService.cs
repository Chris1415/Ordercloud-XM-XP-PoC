using OrderCloud.SDK;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface IOrderService
    {
        string DefaultOrderId { get; }
        IList<Order> GetOrders();
        void RemoveOrder();
        Order GetMyActiveOrder();
        void AddOrUpdateLineItem(string productId, int quantity, string orderId = null);
        void RemoveLineItem(string productId, string orderId = null);
        Order CreateOrder(string orderId = null);
        IList<Order> GetAbandonedOrders();
        bool IsOrderAbandoned(string orderId);
        IList<LineItem> GetOrderLineItems(string orderId);
    }
}
