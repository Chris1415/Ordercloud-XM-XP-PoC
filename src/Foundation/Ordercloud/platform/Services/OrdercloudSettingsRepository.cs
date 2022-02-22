using Sitecore.Data.Items;

namespace BasicCompany.Foundation.Products.Ordercloud.Services
{
    public class OrdercloudSettingsRepository : IOrdercloudSettingsRepository
    {
        public Item GetSettingsItem => Sitecore.Context.Database.GetItem(Constants.Global.SettingsItemId);
        public string GetAuthUrl => GetSettingsItem.Fields[Templates.Settings.Fields.AuthUrl].Value;
        public string GetClientId => GetSettingsItem.Fields[Templates.Settings.Fields.ClientId].Value;
        public string GetClientSecret => GetSettingsItem.Fields[Templates.Settings.Fields.ClientSecret].Value;
        public string GetUsername => GetSettingsItem.Fields[Templates.Settings.Fields.Username].Value;
        public string GetUserPassword => GetSettingsItem.Fields[Templates.Settings.Fields.Password].Value;

        public string ClientSideUsername => GetSettingsItem.Fields[Templates.Settings.Fields.ClientSideUsername].Value;
        public string ClientSidePassword => GetSettingsItem.Fields[Templates.Settings.Fields.ClientSidePassword].Value;
        public string[] ClientSideApiRoles => GetSettingsItem.Fields[Templates.Settings.Fields.ClientSideRoles].Value.Split('|');
    }
}