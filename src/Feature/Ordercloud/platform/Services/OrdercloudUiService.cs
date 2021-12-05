using BasicCompany.Feature.Ordercloud.Models;
using BasicCompany.Foundation.Products.Ordercloud.Services;
using Sitecore.Security.Accounts;
using System.Linq;

namespace BasicCompany.Feature.Ordercloud.Services
{
    public class OrdercloudUiService : IOrdercloudUiService
    {
        private readonly IOrderService _orderService;

        public OrdercloudUiService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public OrdercloudUiViewModel GetDashboardViewModel()
        {
            // TODO Make Editable
            string targetUser = "sitecore\\cHahn";
            using (new UserSwitcher(targetUser, true))
            {
                // Will be provided in next release
                return new OrdercloudUiViewModel()
                {
                    CartId = string.Empty,
                    HasAbandonedCart = false,
                    User = targetUser,
                    Cart = new CartModel()
                    {
                    }
                };
            }
        }
    }
}