using BasicCompany.Foundation.Products.Ordercloud.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Tasks;
using Sitecore.Web.UI.Sheer;
using System;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Commands
{
    public class GetAbandonedCartsCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            int abandonedCarts = ExecuteAbandonedCarts();
            SheerResponse.Alert($"Found {abandonedCarts} carts");
        }

        public void ExecuteScheduled(Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            ExecuteAbandonedCarts();
        }

        private int ExecuteAbandonedCarts()
        {
            int numberOfAbandonedCarts = 0;
            IOrderService _orderService = DependencyResolver.Current.GetService<IOrderService>();
            using (new DatabaseSwitcher(Factory.GetDatabase("master")))
            {
                var abandonedOrders = _orderService.GetAbandonedOrders();
                numberOfAbandonedCarts = abandonedOrders.Count;
                if (abandonedOrders != null && abandonedOrders.Count > 0)
                {
                    foreach (var abandonedOrder in abandonedOrders)
                    {
                        try
                        {
                            string cartsUser = abandonedOrder.xp.ScUser;
                            if (cartsUser != null)
                            {
                                using (new UserSwitcher(cartsUser, true))
                                {
                                    // Will be provided in the next release
                                }
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }

                }
            }

            return numberOfAbandonedCarts;
        }
    }
}