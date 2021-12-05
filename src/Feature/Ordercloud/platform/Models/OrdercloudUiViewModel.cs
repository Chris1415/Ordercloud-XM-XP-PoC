using OrderCloud.SDK;

namespace BasicCompany.Feature.Ordercloud.Models
{
    public class OrdercloudUiViewModel
    {
        public string User { get; set; }
        public bool HasAbandonedCart { get; set; }
        public string CartId { get; set; }
        public CartModel Cart { get; set; }
    }
}