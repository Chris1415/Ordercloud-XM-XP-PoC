using BasicCompany.Feature.Ordercloud.Models;

namespace BasicCompany.Feature.Ordercloud.Services
{
    public interface IOrdercloudUiService
    {
        OrdercloudUiViewModel GetDashboardViewModel();
    }
}
