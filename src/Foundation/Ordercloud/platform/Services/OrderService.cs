using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore.Analytics;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class OrderService : BaseOrdercloudService, IOrderService
    {
        private const string OrderIdFormat = "{0}-{1}-{2}";
        private const string ManualIdentifier = "b804235a-96bf-403e-9a61-2def08b2ea60";
        private const int NumberMinutesTillAbandoned = 2;

        private readonly IOrdercloudAsyncService _ordercloudAsyncService;

        private string OrderId => string.Format(OrderIdFormat, Sitecore.Context.Site.Name, ManualIdentifier /*Tracker.Current.Contact.ContactId*/, "DefaultCart");

        public OrderService(IOrdercloudWebclient ordercloudWebclient,
            IOrdercloudAsyncService ordercloudAsyncService) : base(ordercloudWebclient)
        {
            _ordercloudAsyncService = ordercloudAsyncService;
        }

        public string DefaultOrderId => OrderId;

        public Order CreateOrder(string orderId = null)
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            var newOrder = new Order()
            {
                ID = orderId ?? OrderId
            };

            newOrder.xp = new
            {
                ScUser = Sitecore.Context.User.Name
            };
            Task<Order> task = Task.Run(() => _ordercloudAsyncService.CreateOrderAsync(client, newOrder));
            task.Wait();
            return task.Result;

        }

        public void RemoveLineItem(string productId, string orderId = null)
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            orderId = orderId ?? OrderId;

            LineItem lineItem = null;
            try
            {
                // TODO ProductID Is normally not == LineItemID
                Task<LineItem> task = Task.Run(() => _ordercloudAsyncService.GetLineItemAsync(client, productId, orderId));
                task.Wait();
                lineItem = task.Result;
            }
            catch (Exception e)
            {
            }

            if (lineItem != null)
            {
                Task<bool> task = Task.Run(() => _ordercloudAsyncService.DeleteLineItemAsync(client, orderId, lineItem.ID));
                task.Wait();
                bool success = task.Result;
            }
        }

        public void AddOrUpdateLineItem(string productId, int quantity, string orderId = null)
        {
            orderId = orderId ?? OrderId;
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            LineItem lineItem = null;
            try
            {
                // TODO ProductID Is normally not == LineItemID
                Task<LineItem> task = Task.Run(() => _ordercloudAsyncService.GetLineItemAsync(client, productId, orderId));
                task.Wait();
                lineItem = task.Result;
            }
            catch (Exception e)
            {
            }

            LineItem newLineItem = new LineItem()
            {
                ProductID = productId,
                Quantity = quantity + (lineItem?.Quantity ?? 0)
            };

            Task<LineItem>  addOrUpdateTask = Task.Run(() => _ordercloudAsyncService.CreateOrPatchLineItem(client, OrderDirection.Outgoing, orderId, newLineItem));
            addOrUpdateTask.Wait();          
        }

        public IList<Order> GetOrders()
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            IList<Order> orders = new List<Order>();
            Task<IList<Order>> task = Task.Run(() => _ordercloudAsyncService.GetAsyncOrders(client));
            task.Wait();
            return task.Result;
        }

        public IList<LineItem> GetOrderLineItems(string orderId)
        {
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            Task<IList<LineItem>> task = Task.Run(() => _ordercloudAsyncService.GetLineItemsAsync(client, orderId));
            task.Wait();
            return task.Result;
        }

        public IList<Order> GetAbandonedOrders()
        {
            var orders = GetOrders();
            return orders
                .Where(element => element.LastUpdated.ToLocalTime().DateTime.AddMinutes(NumberMinutesTillAbandoned) < DateTime.Now)
                .ToList();
        }

        public bool IsOrderAbandoned(string orderId)
        {
            var abandonedOrders = GetAbandonedOrders();
            return abandonedOrders.Any(element => element.ID.Equals(orderId));
        }

        public Order GetMyActiveOrder()
        {
            var orders = GetOrders();
            return orders.FirstOrDefault(element => element.ID.Equals(OrderId));
        }

        public void RemoveOrder()
        {
            var orderId = OrderId;
            var client = _ordercloudWebclient.GetClient(new[] { ApiRole.FullAccess });
            Task<bool> task = Task.Run(() => _ordercloudAsyncService.DeleteOrderAsync(client, orderId));
            task.Wait();
        }

        #region Helper

        #endregion
    }
}
