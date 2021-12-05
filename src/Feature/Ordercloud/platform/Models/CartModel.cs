using OrderCloud.SDK;
using System.Collections.Generic;

namespace BasicCompany.Feature.Ordercloud.Models
{
    public class CartModel
    {
        public Order Order { get; set; }
        public IList<LineItem> LineItems { get; set; }

        public CartModel()
        {
            LineItems = new List<LineItem>();
        }
    }
}