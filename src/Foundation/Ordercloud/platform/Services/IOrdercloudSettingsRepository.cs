using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public interface IOrdercloudSettingsRepository
    {
        Item GetSettingsItem { get; }
        string GetAuthUrl { get; }
        string GetClientId { get; }
        string GetClientSecret { get; }
        string GetUsername { get; }
        string GetUserPassword { get; }
    }
}