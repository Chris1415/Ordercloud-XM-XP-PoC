using BasicCompany.Foundation.Products.Ordercloud.Webclients;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Base
{
    public class BaseOrdercloudService : IBaseOrdercloudService
    {
        protected readonly IOrdercloudWebclient _ordercloudWebclient;

        public BaseOrdercloudService(IOrdercloudWebclient ordercloudWebclient)
        {
            _ordercloudWebclient = ordercloudWebclient;
        }
    }
}