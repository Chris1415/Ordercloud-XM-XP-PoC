using Sitecore.Data;

namespace BasicCompany.Foundation.Products.Ordercloud
{
    public static class Templates
    {
        public static class Settings
        {
            public static readonly ID ID = new ID("{CE046CA7-2C10-45E7-9214-3A8829A15AC6}");
            public struct Fields
            {
                public static readonly ID AuthUrl = new ID("{18E5B8B8-1093-45FE-A3D1-49FCED88E97E}");
                public static readonly ID ClientId = new ID("{D2A39A27-90F2-4AAF-A7E2-8C1FFBC32C08}");
                public static readonly ID ClientSecret = new ID("{29E37891-4002-4DBC-8E52-22554AAF2B06}");
                public static readonly ID Username = new ID("{5888304C-162E-4E29-A8F8-018038407A88}");
                public static readonly ID Password = new ID("{EF1FB986-D333-4B7A-A578-8F6A2796AD0C}");

                public static readonly ID CatalogContentRoot = new ID("{088EA09B-28DE-4023-ACFA-BF67DCEAC0E7}");
                public static readonly ID Catalog = new ID("{1BAA5D80-3358-4CF8-BCE3-3076CE74D884}");

                public static readonly ID ClientSideUsername = new ID("{5073DC66-4CE3-4451-AAC9-7938397728A6}");
                public static readonly ID ClientSidePassword = new ID("{40F72B5D-D48B-4124-80B4-371AC4279FA4}");
                public static readonly ID ClientSideRoles = new ID("{2BA9E7D8-449A-43DD-9064-2A5566F9C16F}");
            }
        }

        public static class GlobalFolder
        {
            public static readonly ID ID = new ID("{D139ADC1-DAA8-40B8-814E-1722F6ED88E5}");
        }
    }
}