import { Button } from '@material-ui/core';
import * as React from 'react';
import { makeStyles } from '@material-ui/core/styles';

const style = makeStyles({
  SpaceSRight: {
    marginRight: '10px',
  },
});

const ProductInteractionButtons = (productId: string): JSX.Element => {
  const classes = style();
  return (
    <div className="container">
      <div className="row">
        <div className="col-12">
          <b>Test Buttons for Ordercloud interactions</b>
        </div>
        <div className="col-12">
          <Button
            className={classes.SpaceSRight}
            variant="contained"
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
          </Button>
          <Button
            className={classes.SpaceSRight}
            variant="contained"
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
          </Button>
          <Button
            className={classes.SpaceSRight}
            variant="contained"
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
          </Button>
        </div>
      </div>
    </div>
  );
};

export default ProductInteractionButtons;
