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
                public static readonly ID Password = new ID("{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}");

                public static readonly ID CatalogContentRoot = new ID("{088EA09B-28DE-4023-ACFA-BF67DCEAC0E7}");
                public static readonly ID Catalog = new ID("{1BAA5D80-3358-4CF8-BCE3-3076CE74D884}");
            }
        }

        public static class GlobalFolder
        {
            public static readonly ID ID = new ID("{D139ADC1-DAA8-40B8-814E-1722F6ED88E5}");
        }
    }
}