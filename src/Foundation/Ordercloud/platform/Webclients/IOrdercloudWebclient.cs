using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Webclients
{
    public interface IOrdercloudWebclient
    {
        OrderCloudClient GetClient(ApiRole[] apiRoles, GrantType grantType = GrantType.ClientCredentials);
        OrderCloudClient GetUserClient(ApiRole[] apiRoles = null, string username = null, string password = null, GrantType grantType = GrantType.Password);
    }

}