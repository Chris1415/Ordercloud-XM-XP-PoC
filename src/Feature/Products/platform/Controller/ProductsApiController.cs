using BasicCompany.Feature.Products.Models.Webhooks;
using BasicCompany.Foundation.Products.Ordercloud.Commands;
using BasicCompany.Foundation.Products.Ordercloud.Customer.Models;
using BasicCompany.Foundation.Products.Ordercloud.Customer.Services.Importer;
using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.SecurityModel;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;

namespace BasicCompany.Feature.Products.Controller
{
    public class ProductsApiController : System.Web.Mvc.Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomProductImportService _productImportService;
        private const string DummyUser = "sitecore\\cHahn";

        public ProductsApiController(IOrderService orderService,
            IProductService productService,
            ICustomProductImportService productImportService)
        {
            _orderService = orderService;
            _productService = productService;
            _productImportService = productImportService;
        }

        [HttpPost]
        public ActionResult UpdateWebhook()
        {
            var bodyStream = new StreamReader(HttpContext.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            var webhookModel = JsonConvert.DeserializeObject<ProductWebhookModel>(bodyText);
            if(webhookModel != null)
            {
                var productToUpdate = webhookModel.Response.Body.ID;
                using (new DatabaseSwitcher(Factory.GetDatabase("master")))
                {
                    using (new SecurityDisabler())
                    {
                        _productImportService.Import<MyProduct>(productToUpdate);
                    }
                }
            }
            
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult AddLineItemToCart(string productId)
        {
            var item = _productService.GetProduct(productId);
            if (item == null)
            {
                return Json("Not Found", JsonRequestBehavior.AllowGet);
            }

            if (Sitecore.Analytics.Tracker.Current == null)
            {
                Sitecore.Analytics.Tracker.Initialize();
                Sitecore.Analytics.Tracker.StartTracking();
            }

            if (!Sitecore.Context.User.IsAuthenticated)
            {
                bool isLoggedIn = AuthenticationManager.Login(DummyUser, true);
            }

            var identificationManager = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetRequiredService<Sitecore.Analytics.Tracking.Identification.IContactIdentificationManager>();
            Sitecore.Analytics.Tracking.Identification.IdentificationResult result = identificationManager.IdentifyAs(new Sitecore.Analytics.Tracking.Identification.KnownContactIdentifier("usernamesource", DummyUser));


            var myActiveOrder = _orderService.GetMyActiveOrder();
            if (myActiveOrder == null)
            {
                _orderService.CreateOrder();
            }

            _orderService.AddOrUpdateLineItem(item.Name, 1);
            return Json($"Success - Added {item.Name} to cart - {_orderService.DefaultOrderId} - ContactID: {Sitecore.Analytics.Tracker.Current.Contact.ContactId}", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateOrUpdateContact()
        {
            var membershipUser = Membership.GetUser(DummyUser);
            if (membershipUser == null)
            {
                Membership.CreateUser(DummyUser, "Test", "christian.hahn@sitecore.com");
                User newUser = Sitecore.Security.Accounts.User.FromName(DummyUser, true);
                Sitecore.Security.UserProfile userProfile = newUser.Profile;
                userProfile.FullName = string.Format("{0} {1}", "Christian", "Hahn");
                userProfile.Save();
            }

            if (!Sitecore.Context.User.IsAuthenticated)
            {
                bool isLoggedIn = AuthenticationManager.Login(DummyUser, true);
            }

            // TODO Make Better ;-) 
            var user = Sitecore.Context.User;
            return Json($"Success - Created or updated user {user.Name}", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveLineItemFromCart(string productId)
        {
            if (!ID.TryParse(productId, out var itemId))
            {
                return Json("Not Found", JsonRequestBehavior.AllowGet);
            }

            var item = Sitecore.Context.Database.GetItem(itemId);
            if (item == null)
            {
                return Json("Not Found", JsonRequestBehavior.AllowGet);
            }

            if (!Sitecore.Context.User.IsAuthenticated)
            {
                AuthenticationManager.Login(DummyUser, true);
            }

            var identificationManager = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetRequiredService<Sitecore.Analytics.Tracking.Identification.IContactIdentificationManager>();
            Sitecore.Analytics.Tracking.Identification.IdentificationResult result = identificationManager.IdentifyAs(new Sitecore.Analytics.Tracking.Identification.KnownContactIdentifier("usernamesource", DummyUser));

            var myActiveOrder = _orderService.GetMyActiveOrder();
            if (myActiveOrder != null)
            {
                _orderService.RemoveLineItem(item.Name);
            }

            HttpContext.Session.Abandon();
            Sitecore.Analytics.Tracker.Current.EndVisit(true);
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TriggerBuy(int value)
        {
            if (Sitecore.Analytics.Tracker.Current == null)
            {
                Sitecore.Analytics.Tracker.Initialize();
                Sitecore.Analytics.Tracker.StartTracking();
            }

            if (!Sitecore.Context.User.IsAuthenticated)
            {
                AuthenticationManager.Login(DummyUser, true);
            }

            var identificationManager = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetRequiredService<Sitecore.Analytics.Tracking.Identification.IContactIdentificationManager>();
            Sitecore.Analytics.Tracking.Identification.IdentificationResult result = identificationManager.IdentifyAs(new Sitecore.Analytics.Tracking.Identification.KnownContactIdentifier("usernamesource", DummyUser));
            _orderService.RemoveOrder();

            return Json($"Triggered Order with value - {value}", JsonRequestBehavior.AllowGet);
        }

        public ActionResult EndVisit()
        {
            HttpContext.Session.Abandon();
            Sitecore.Analytics.Tracker.Current.EndVisit(true);
            Sitecore.Analytics.Tracker.Current.EndTracking();
            return Json($"Ended Visit", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckAbandonedCarts()
        {
            var command = new GetAbandonedCartsCommand();
            command.Execute(new Sitecore.Shell.Framework.Commands.CommandContext());
            Sitecore.Analytics.Tracker.Current.EndVisit(true);
            Sitecore.Analytics.Tracker.Current.EndTracking();
            return Json($"Done", JsonRequestBehavior.AllowGet);
        }
    }
}