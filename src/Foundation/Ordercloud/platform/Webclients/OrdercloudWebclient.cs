using BasicCompany.Foundation.Products.Ordercloud.Extensions;
using BasicCompany.Foundation.Products.Ordercloud.Services;
using OrderCloud.SDK;


namespace BasicCompany.Foundation.Products.Ordercloud.Webclients
{
    public class OrdercloudWebclient : IOrdercloudWebclient
    {
        private OrderCloudClient _orderCloudBEClient;
        private OrderCloudClient _orderCloudFEClient;
        private readonly IOrdercloudSettingsRepository _ordercloudSettingsRepository;

        public OrdercloudWebclient(IOrdercloudSettingsRepository ordercloudSettingsRepository)
        {
            _ordercloudSettingsRepository = ordercloudSettingsRepository;
        }

        public OrderCloudClient GetClient(ApiRole[] apiRoles, GrantType grantType = GrantType.ClientCredentials)
        {
            if (_orderCloudBEClient == null)
            {
                _orderCloudBEClient = new OrderCloudClient(new OrderCloudClientConfig
                {
                    ClientId = _ordercloudSettingsRepository.GetClientId,
                    ClientSecret = _ordercloudSettingsRepository.GetClientSecret,
                    Username = _ordercloudSettingsRepository.GetUsername,
                    Password = _ordercloudSettingsRepository.GetUserPassword,
                    GrantType = grantType,
                    Roles = apiRoles,
                    ApiUrl = _ordercloudSettingsRepository.GetAuthUrl,
                    AuthUrl = _ordercloudSettingsRepository.GetAuthUrl
                });
            }

            return _orderCloudBEClient;
        }

        public OrderCloudClient GetUserClient(ApiRole[] apiRoles = null, string username = null, string password = null, GrantType grantType = GrantType.ClientCredentials)
        {
            if (_orderCloudFEClient == null)
            {
                _orderCloudFEClient = new OrderCloudClient(new OrderCloudClientConfig
                {
                    ClientId = _ordercloudSettingsRepository.GetClientId,
                    ClientSecret = _ordercloudSettingsRepository.GetClientSecret,
                    Username = username ?? _ordercloudSettingsRepository.GetUsername,
                    Password = password ?? _ordercloudSettingsRepository.GetUserPassword,
                    GrantType = grantType,
                    Roles = apiRoles ?? _ordercloudSettingsRepository.ClientSideApiRoles.ToApiRoles(),
                    ApiUrl = _ordercloudSettingsRepository.GetAuthUrl,
                    AuthUrl = _ordercloudSettingsRepository.GetAuthUrl
                });
            }

            return _orderCloudFEClient;
        }
    }
}