using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Webclients
{
    public interface IOrdercloudWebclient
    {
        OrderCloudClient GetClient(ApiRole[] apiRoles, GrantType grantType = GrantType.ClientCredentials);
        OrderCloudClient GetClient(ApiRole[] apiRoles, string username, string password, GrantType grantType = GrantType.Password);
    }

}