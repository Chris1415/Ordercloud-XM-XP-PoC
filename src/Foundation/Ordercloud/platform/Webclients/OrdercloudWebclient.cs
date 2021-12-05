using BasicCompany.Foundation.Products.Ordercloud.Services;
using OrderCloud.SDK;


namespace BasicCompany.Foundation.Products.Ordercloud.Webclients
{
    public class OrdercloudWebclient : IOrdercloudWebclient
    {
        private OrderCloudClient _orderCloudClient;
        private readonly IOrdercloudSettingsRepository _ordercloudSettingsRepository;

        public OrdercloudWebclient(IOrdercloudSettingsRepository ordercloudSettingsRepository)
        {
            _ordercloudSettingsRepository = ordercloudSettingsRepository;
        }

        public OrderCloudClient GetClient(ApiRole[] apiRoles, GrantType grantType = GrantType.ClientCredentials)
        {
            if (_orderCloudClient == null)
            {
                _orderCloudClient = new OrderCloudClient(new OrderCloudClientConfig
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

            return _orderCloudClient;
        }

        public OrderCloudClient GetClient(ApiRole[] apiRoles, string username, string password, GrantType grantType = GrantType.Password)
        {
            if (_orderCloudClient == null)
            {
                _orderCloudClient = new OrderCloudClient(new OrderCloudClientConfig
                {
                    ClientId = _ordercloudSettingsRepository.GetClientId,
                    ClientSecret = string.Empty,
                    Username = username,
                    Password = password,
                    GrantType = grantType,
                    Roles = apiRoles,
                    ApiUrl = _ordercloudSettingsRepository.GetAuthUrl,
                    AuthUrl = _ordercloudSettingsRepository.GetAuthUrl
                });
            }

            return _orderCloudClient;
        }
    }
}