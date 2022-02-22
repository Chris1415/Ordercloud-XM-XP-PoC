import * as React from 'react';

const ProductInteractionButtons = (productId: string): JSX.Element => {
  return (
    <div className="container">
      <div className="row">
        <div className="col-12">
          <h3>Test Buttons for Ordercloud interactions</h3>
        </div>
        <div className="col-4">
          <div
            className="SitecoreButton SpaceSRight"
            onClick={() => {
              (window as { [key: string]: any })['Ajax'](
                'https://cm.jss18demo.localhost/api/sitecore/productsapi/AddLineItemToCart?productid=' +
                  productId,
                function (data: any) {
                  (window as { [key: string]: any })['PlaceMessage'](data);
                }
              );
            }}
          >
            Add to Cart
          </div>
        </div>
        <div className="col-4">
          <div
            className="SitecoreButton SpaceSRight"
            onClick={() => {
              (window as { [key: string]: any })['Ajax'](
                'https://cm.jss18demo.localhost/api/sitecore/productsapi/RemoveLineItemFromCart?productid=' +
                  productId,
                function (data: any) {
                  (window as { [key: string]: any })['PlaceMessage'](data);
                }
              );
            }}
          >
            Remove from Cart
          </div>
        </div>
        <div className="col-4">
          <div
            className="SitecoreButton SpaceSRight"
            onClick={() => {
              (window as { [key: string]: any })['Ajax'](
                'https://cm.jss18demo.localhost/api/sitecore/productsapi/TriggerBuy?value=' + 50,
                function (data: any) {
                  (window as { [key: string]: any })['PlaceMessage'](data);
                }
              );
            }}
          >
            Submit Order
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductInteractionButtons;
