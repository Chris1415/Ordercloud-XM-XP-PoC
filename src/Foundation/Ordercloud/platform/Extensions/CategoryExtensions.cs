using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicCompany.Foundation.Products.Ordercloud.Extensions
{
    public static class CategoryExtensions
    {
        public static Category ToCategory(this PartialCategory partialCategory)
        {
            return new Category()
            {
                Active = partialCategory.Active,
                Description = partialCategory.Description,
                ID = partialCategory.ID,
                Name = partialCategory.Name,
                ParentID = partialCategory.ParentID,
            };
        }

        public static PartialCategory ToPartialCategory(this Category category)
        {
            return new PartialCategory()
            {
                Active = category.Active,
                Description = category.Description,
                ID = category.ID,
                Name = category.Name,
                ParentID = category.ParentID,
            };
        }
    }
}