using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Extensions
{
    public static class LineItemExtensions
    {
        public static LineItem ToLineItem(this PartialLineItem partialLineItem)
        {
            return new LineItem()
            {
                ProductID = partialLineItem.ProductID,
                Quantity = partialLineItem.Quantity
            };   
        }

        public static PartialLineItem ToPartialLineItem(this LineItem lineItem)
        {
            return new PartialLineItem()
            {
                ProductID = lineItem.ProductID,
                Quantity = lineItem.Quantity
            };
        }
    }
}