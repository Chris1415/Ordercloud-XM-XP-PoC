using BasicCompany.Foundation.Products.Ordercloud.Services.Base;
using BasicCompany.Foundation.Products.Ordercloud.Webclients;
using OrderCloud.SDK;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public class CategoryImportService : BaseOrdercloudService, ICategoryImportService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductReferenceImportService _productReferenceImportService;

        public CategoryImportService(
            IOrdercloudWebclient ordercloudWebclient,
            ICategoryService categoryService,
            IProductReferenceImportService productReferenceImportService) : base(ordercloudWebclient)
        {
            _categoryService = categoryService;
            _productReferenceImportService = productReferenceImportService;
        }

        public bool Import(string catalogId, Item catalogItem)
        {
            var client = _ordercloudWebclient.GetClient(new ApiRole[] { ApiRole.FullAccess });

            var categories = client.Categories.ListAsync(catalogId, "all").Result;
            foreach (var category in categories.Items)
            {
                string id = category.ID;
                string description = category.Description;
                int childCount = category.ChildCount;
                int? listOrder = category.ListOrder;
                string displayName = category.Name;
                string name = ItemUtil.ProposeValidItemName(displayName);
                string parentId = category.ParentID;
                bool isActive = category.Active;

                var rootItem = _categoryService.GetCategoryItem(catalogItem, parentId) ?? catalogItem;
                var existingItem = rootItem.Children.FirstOrDefault(element => element.Name.Equals(id));
                if (existingItem == null)
                {
                    using (new EventDisabler())
                    {
                        existingItem = rootItem.Add(id, new TemplateID(Products.Templates.Category.ID));
                    }
                }

                using (new EventDisabler())
                {
                    existingItem.Editing.BeginEdit();
                    existingItem[Products.Templates.Category.Fields.Name] = name;
                    existingItem[Products.Templates.Base.Fields.DisplayName] = displayName;
                    existingItem[Products.Templates.Category.Fields.Id] = id;
                    existingItem[Products.Templates.Category.Fields.Description] = description;
                    existingItem[Products.Templates.Category.Fields.IsActive] = isActive ? "1" : "0";
                    existingItem[Products.Templates.Category.Fields.ChildCount] = childCount.ToString();
                    existingItem.Editing.EndEdit();
                }

                _productReferenceImportService.Import(id, catalogId, existingItem);
            }

            return true;
        }
    }
}
