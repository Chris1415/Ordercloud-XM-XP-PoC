using Sitecore.Data;

namespace BasicCompany.Foundation.Products
{
    public static class Templates
    {
        public static class MediaFolder
        {
            public static readonly ID ID = new ID("{FE5DD826-48C6-436D-B87A-7C4210C7413B}");
        }

        public static class Base
        {
            public struct Fields
            {
                public static readonly ID DisplayName = new ID("{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}");
            }
        }

        public static class Catalog
        {
            public static readonly ID ID = new ID("{C04F0307-3FA9-4CE1-8CD1-C655A31FC2DB}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{501DFC41-0856-4FD8-9302-1FC738813E91}");
                public static readonly ID Id = new ID("{B08BE566-EFAF-42FC-848F-014EECEE84F7}");
                public static readonly ID Description = new ID("{A81A0E7F-8FAC-4438-BCDD-6C495B847979}");
                public static readonly ID IsActive = new ID("{7BA25A6F-773A-45E3-87DF-D5A92C10E09F}");
                public static readonly ID OwnerId = new ID("{F6257D5F-46A2-432B-B4B4-2829971529CE}");
                public static readonly ID CategoryCount = new ID("{60BE861F-B2BB-48FF-B8EE-B3A447336953}");
            }
        }

        public static class Category
        {
            public static readonly ID ID = new ID("{0826D4FA-C48D-406A-9EA9-EA168F99612F}");

            public struct Fields
            {
                public static readonly ID Id = new ID("{41827A00-6A2B-409F-BC5A-541B45432510}");
                public static readonly ID Name = new ID("{BA88D4B9-8CB2-4568-AACB-8D6BBE843EC2}");
                public static readonly ID Description = new ID("{56F4C1B3-95BE-4B36-8DC4-FE54CE6816BE}");
                public static readonly ID IsActive = new ID("{7DC2E76B-FD08-443C-9D24-19C4DC2DA52C}");
                public static readonly ID ChildCount = new ID("{20820BBA-4380-4DD7-B2FD-24B45C2EE590}");
            }
        }

        public static class ProductReference
        {
            public static readonly ID ID = new ID("{6F2E2015-355C-4439-9BE5-FC7518A0F1BB}");

            public struct Fields
            {
                public static readonly ID ProductReference = new ID("{CE4F9F72-AA64-42A4-AB48-B5E12C83D0ED}");
            }
        }

        public static class Product
        {
            public static readonly ID ID = new ID("{28E2B474-21C1-48F4-A4A8-B95ED59FB307}");

            public struct Fields
            {
                public static readonly ID QuantityMultiplier = new ID("{D1BA71D6-43E3-44FC-90FB-B0679645840E}");
                public static readonly ID ShipFromAddress = new ID("{1ACF6AEE-25F0-4DE5-9180-7060F545D42B}");
                public static readonly ID AutoForward = new ID("{85661E95-33FD-4F2C-8BAE-BE59B5D0F0CD}");
                public static readonly ID DefaultSupplier = new ID("{B9392385-BD1D-4AD4-9CA5-D640DF51332A}");
                public static readonly ID AllSupplierCanSell = new ID("{5E6AE8CE-D46C-4D45-9953-793D06D4FCFC}");

                public static readonly ID Enabled = new ID("{00AF71A1-5CD0-4D4A-8C43-2654C2537D6D}");
                public static readonly ID NotificationPoint = new ID("{6C0F4462-276D-4BBE-B478-B54A29B9D366}");
                public static readonly ID VariantLevelTracking = new ID("{2192F305-260C-4EA9-B8D5-868200348BD0}");
                public static readonly ID OrderCanExceed = new ID("{BFD73A1B-9AC1-448C-93A9-450DEDB54738}");
                public static readonly ID QuantityAvailable = new ID("{8E3EAFB3-AE56-4377-ABBD-6A25A85B0275}");

                public static readonly ID ProductSpecs = new ID("{73061D32-2EB0-4242-96CF-62FFF2502DDA}");
            }
        }

        public static class ProductData
        {
            public static readonly ID ID = new ID("{80053541-A7A9-4925-8AD0-2B14DB99A47C}");

            public struct Fields
            {
                public static readonly ID Weight = new ID("{A5D3EFE4-B60B-4A1C-8710-69DE0B9631FA}");
                public static readonly ID Height = new ID("{39A8B138-DBA7-4ECA-B69E-31810B449EF1}");
                public static readonly ID Width = new ID("{231B9594-6B85-4F87-9B71-EFA9CED2CC22}");
                public static readonly ID Length = new ID("{BFD73A1B-9AC1-448C-93A9-450DEDB54738}");

                public static readonly ID IsActive = new ID("{390016BA-0A5E-4E3B-9994-AEF90477042C}");
                public static readonly ID Name = new ID("{655D9F09-66F0-4067-A226-711C40A26ED7}");
                public static readonly ID Id = new ID("{7D03B3D1-84A1-4A61-8599-2BAEF4B80C65}");
                public static readonly ID Description = new ID("{41325A41-05FA-4E6B-BF8D-898AF4A6D60E}");
            }
        }

        public static class Spec
        {
            public static readonly ID ID = new ID("{D2397339-7977-4E25-9338-6D44F4007FE8}");

            public struct Fields
            {
                public static readonly ID Id = new ID("{FE1CC9AB-2771-4E47-A959-FD983A0D8E38}");
                public static readonly ID Name = new ID("{CFA4076B-5D86-450C-9898-E3E95AA12653}");
                public static readonly ID Required = new ID("{1229927E-0D23-4165-AC27-A7A3EC97DF74}");
                public static readonly ID DefinesVariant = new ID("{A88AE579-F9DA-4BF1-BBE3-6F0A1B3E5670}");
            }
        }

        public static class SpecOptionReference
        {
            public static readonly ID ID = new ID("{A9776D70-37C3-43E3-B150-D88ED28A08FA}");

            public struct Fields
            {
                public static readonly ID SpecReference = new ID("{4181826B-6A48-4C0E-B44B-66F9279E0A05}");
            }
        }

        public static class SpecOption
        {
            public static readonly ID ID = new ID("{9845F598-CBFA-4A11-8857-16331ECE68F4}");

            public struct Fields
            {
                public static readonly ID Id = new ID("{F2544189-0CCF-491C-A912-5B1E77537649}");
                public static readonly ID Value = new ID("{83D00647-725F-4668-AE83-83B35ECEF539}");
                public static readonly ID PriceMarkupType = new ID("{3B9FC33A-5B4E-485D-A608-975487B2263B}");
                public static readonly ID PriceMarkup = new ID("{DC5381EE-1942-4C1A-8DE2-8B0DA0DB22F8}");
            }
        }

        public static class ProductVariant
        {
            public static readonly ID ID = new ID("{E883EB75-8AF6-4EDA-8BEB-ECD29BA7D08E}");

            public struct Fields
            {
                public static readonly ID Specs = new ID("{A5D3EFE4-B60B-4A1C-8710-69DE0B9631FA}");
            }
        }

        public static class ProductCmsExtension
        {
            public static readonly ID ID = new ID("{94C5C742-3963-435E-B9FA-20C7E7C592D4}");

            public struct Fields
            {
                public static readonly ID Mimes = new ID("{85145F44-E2F1-4FA4-9DD5-32D414B22DC8}");
                public static readonly ID Image = new ID("{4B633F05-0318-4C9E-99B6-1FD642B562E8}");
            }
        }

        public static class Price
        {
            public static readonly ID ID = new ID("{8026DFC9-F936-4F7D-9EDE-67A97568E6AF}");

            public struct Fields
            {
                public static readonly ID Amount = new ID("{BBC938D2-671F-4930-9590-6E21EAE64E48}");
                public static readonly ID Currency = new ID("{4B33E5E9-DBE9-4881-9D94-224E41FA3E82}");
                public static readonly ID Tax = new ID("{F0F4EEB0-40E8-41A4-B751-85F2888586B2}");
            }
        }

        public static class Supplier
        {
            public static readonly ID ID = new ID("{DFAF5424-CC6F-4701-BB1B-DA3F90755FE4}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{F5F814F2-E829-4E56-ADE9-375CDAB02169}");
                public static readonly ID Id = new ID("{D7C334DF-62B4-4CB8-A04F-07E2778D20AF}");
                public static readonly ID IsActive = new ID("{8E450629-7859-4BB6-8CEB-575C7614B70C}");
                public static readonly ID AllBuyersCanOrder = new ID("{4245FB6F-C21F-43BC-AE39-64B42B018F30}");
            }
        }

        public static class SupplierBranch
        {
            public static readonly ID ID = new ID("{1E454473-6237-4513-80CB-A0A187CA6537}");
        }

        public static class BuyerBranch
        {
            public static readonly ID ID = new ID("{C0BD553B-8978-4389-BF3C-F097CE922EA1}");
        }

        public static class SupplierUser
        {
            public static readonly ID ID = new ID("{7501327C-D055-4AE5-A5A5-8A879ABD51A1}");
        }

        public static class Seller
        {
            public static readonly ID ID = new ID("{10A430A6-AF90-42B8-8E28-ACD3F64CF8D4}");
        }


        public static class User
        {
            public static readonly ID ID = new ID("{89EEAD7D-EEC7-4897-8D2A-D441788436C3}");
            public struct Fields
            {
                public static readonly ID Id = new ID("{CBB1A9DC-8C27-4AE1-8907-589E994278E4}");
                public static readonly ID Username = new ID("{A160CE5E-538C-49BF-9AA6-1E26C717FAA0}");
                public static readonly ID FirstName = new ID("{9D660863-0795-48E5-99D4-63A083BF1AFB}");
                public static readonly ID LastName = new ID("{1B0D4DC4-5E08-422E-92A6-B736B8D4A525}");
                public static readonly ID Email = new ID("{4213CA21-795D-48D3-9518-E47B2A4639D0}");
                public static readonly ID Phone = new ID("{24F20400-25B3-4215-A016-22E14E7101C7}");
                public static readonly ID TermsAccepted = new ID("{83A734A4-4401-43BB-9BFD-E2B00B4B843D}");
                public static readonly ID IsActive = new ID("{491E1676-D570-4FF8-95F7-B0AFED026C81}");
                public static readonly ID Password = new ID("{E5076661-9BB7-4EA5-9659-CAE14A17A543}");
            }
        }

        public static class SupplierUserGroup
        {
            public static readonly ID ID = new ID("{EEA41377-27D2-486E-9C92-6796DBDA05D6}");
            public struct Fields
            {
                public static readonly ID Id = new ID("{EEA41377-27D2-486E-9C92-6796DBDA05D6}");
                public static readonly ID Name = new ID("{DA70F2EA-5478-454D-A6BB-77C09C2284E4}");
                public static readonly ID Description = new ID("{10DA553F-029C-4FA2-A397-21F34466173D}");
            }
        }

        public static class Buyer
        {
            public static readonly ID ID = new ID("{7B1AF6F9-A1BA-435D-A789-4890528437FE}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{9A7D0D8C-F10C-41DA-B466-1CFF9388A95D}");
                public static readonly ID Id = new ID("{913A947A-17B9-4C7D-9AC9-EA3E20FB83F0}");
                public static readonly ID IsActive = new ID("{6C8C7CDB-5EF0-4D76-BA7C-76F968C30F34}");
            }
        }

        public static class BuyerUser
        {
            public static readonly ID ID = new ID("{7DBBBB38-FF50-4C3C-AD75-F0F591E3F349}");
            public struct Fields
            {
                public static readonly ID Addresses = new ID("{E7522723-94DE-4396-B204-F38175B5CD43}");
            }
        }

        public static class BuyerUserFolder
        {
            public static readonly ID ID = new ID("{52CD36F5-771F-463C-8412-265BA9FA432B}");
        }

        public static class BuyerUserGroup
        {
            public static readonly ID ID = new ID("{BB91CA4B-B1D2-461C-9AA2-0F1BF2C587FD}");
            public struct Fields
            {
                public static readonly ID Id = new ID("{63A7A21F-EF4C-4F54-AE42-C9E5DE0FC6B2}");
                public static readonly ID Name = new ID("{5EC83037-4B9E-4936-9128-4986DA8F636E}");
                public static readonly ID Description = new ID("{5AB92BBD-9776-4154-96D6-F2E2B8DE7A3B}");
                public static readonly ID Users = new ID("{F59367B7-5CEC-415E-B5A1-246C64D8FF03}");
            }
        }

        public static class BuyerUserGroupFolder
        {
            public static readonly ID ID = new ID("{8F80A0B4-2034-4F67-9B6F-D85E780E78D4}");
        }

        public static class Address
        {
            public static readonly ID ID = new ID("{1B028AB4-A17A-42D6-8A68-A8D225807217}");
            public struct Fields
            {
                public static readonly ID AddressName = new ID("{55883C90-F1E6-4275-A5D5-2E060AC301E3}");
                public static readonly ID State = new ID("{D18522DA-AEBA-4AE5-B779-60D8DB6C19F3}");
                public static readonly ID Street = new ID("{965711B4-0FAB-43C3-A2B4-B267D9505C35}");
                public static readonly ID Zip = new ID("{CC8F944E-BF92-4638-91E8-F8B79652779B}");
                public static readonly ID City = new ID("{7D45824B-4168-4571-83C8-6CD289DF0FBC}");
                public static readonly ID Country = new ID("{A8E81959-40FD-430E-8A58-49686A9D55DC}");
                public static readonly ID Phone = new ID("{C4BFB7FC-292F-46C6-9194-9AA33476036C}");
                public static readonly ID FirstName = new ID("{3C86BAB1-A3E0-462E-A0B8-01DC9F6B6309}");
                public static readonly ID LastName = new ID("{DBA28D5C-E779-4DDF-9BC1-5A772F96FFF6}");
                public static readonly ID Street2 = new ID("{2CE41F6F-FC8C-4B2F-960E-6627ABBF831D}");
                public static readonly ID CompanyName = new ID("{570748A6-605B-44C8-802D-64CD93DD1251}");
                public static readonly ID Id = new ID("{0ABCDF49-2C7D-42D2-B8A9-E12D7E100D72}");
            }
        }

        public static class AddressAssignmentsFolder
        {
            public static readonly ID ID = new ID("{E4B0363F-D82C-4B9D-8651-1F3B10B816EA}");
        }

        public static class AddressAssignment
        {
            public static readonly ID ID = new ID("{E89DBFC3-9225-4727-9374-A2B919A10A6A}");
            public struct Fields
            {
                public static readonly ID Address = new ID("{B11019B8-DA2A-4893-818E-563E14E82975}");
                public static readonly ID BuyerUserGroup = new ID("{FE38D2B1-F42E-42B5-8419-1A541F7F84DD}");
                public static readonly ID BuyerUser = new ID("{E936F568-F381-44BA-9B9B-15F02A92B840}");
                public static readonly ID IsShipping = new ID("{786E537F-2CE6-4D03-B1DA-9D6E82F0A356}");
                public static readonly ID IsBilling = new ID("{B4067928-BDD2-411D-82ED-1C2DEF211211}");
            }
        }

        public static class AddressFolder
        {
            public static readonly ID ID = new ID("{186432A6-18FB-4171-90C3-3DA4D7643B28}");
        }

        public static class ProductCategoryPage
        {
            public static readonly ID ID = new ID("{C87F8BF0-9819-4AEC-B30B-D7D1A55D25BB}");
            public struct Fields
            {
                public static readonly ID CategoryReference = new ID("{5317E68A-DADA-4702-A8B3-2B2C5CE95802}");
            }
        }

        public static class ProductDetailPage
        {
            public static readonly ID ID = new ID("{BBF7D65E-47FD-4AAE-BAC2-FFD7A751A840}");
            public struct Fields
            {
                public static readonly ID ProductReference = new ID("{4DAA5418-5002-47DC-B1BD-85B270BC8299}");
            }
        }
    }
}