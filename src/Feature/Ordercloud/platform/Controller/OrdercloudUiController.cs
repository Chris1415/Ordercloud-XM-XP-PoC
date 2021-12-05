using BasicCompany.Feature.Ordercloud.Services;
using System.Web.Mvc;


namespace BasicCompany.Feature.Ordercloud.Controller
{
    public class OrdercloudUiController : System.Web.Mvc.Controller
    {
        private readonly IOrdercloudUiService _ordercloudUiService;

        public OrdercloudUiController(IOrdercloudUiService ordercloudUiService)
        {
            _ordercloudUiService = ordercloudUiService;
        }

        public ActionResult OrdercloudDashboard()
        {
            return View("~/Views/Feature/OrdercloudUi.cshtml", _ordercloudUiService.GetDashboardViewModel());
        }
       
    }
}