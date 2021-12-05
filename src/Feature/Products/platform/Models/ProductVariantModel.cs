using System.Collections.Generic;

namespace BasicCompany.Feature.Products.Models
{
    public class ProductVariantModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IList<SpecModel> Specs { get; set; }
    }
}