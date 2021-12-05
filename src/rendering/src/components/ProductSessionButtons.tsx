import { Button } from '@material-ui/core';
import * as React from 'react';
import { makeStyles } from '@material-ui/core/styles';

const style = makeStyles({
  SpaceSRight: {
    marginRight: '10px',
  },
});

const ProductSessionButtons = (render: boolean): JSX.Element => {
  const classes = style();
  if (!render) {
    return <></>;
  } else {
    return (
      <>
        <hr />
        <div className="container">
          <div className="row">
            <div className="col-12">
              <b>Test Buttons for Session/Facet information</b>
            </div>
            <div className="col-12">
              <Button
                className={classes.SpaceSRight}
                variant="contained"
                onClick={() => {
                  (window as { [key: string]: any })['Ajax'](
                    'https://cm.jss18demo.localhost/api/sitecore/productsapi/CheckAbandonedCarts',
                    function (data: any) {
                      (window as { [key: string]: any })['PlaceMessage'](data);
                    }
                  );
                }}
              >
                Abandoned Cart
              </Button>
              <Button
                className={classes.SpaceSRight}
                variant="contained"
                onClick={() => {
                  (window as { [key: string]: any })['Ajax'](
                    'https://cm.jss18demo.localhost/api/sitecore/productsapi/CreateOrUpdateContact',
                    function (data: any) {
                      (window as { [key: string]: any })['PlaceMessage'](data);
                    }
                  );
                }}
              >
                Create or Update Contact in Sitecore
              </Button>
              <Button
                className={classes.SpaceSRight}
                variant="contained"
                onClick={() => {
                  (window as { [key: string]: any })['Ajax'](
                    'https://cm.jss18demo.localhost/api/sitecore/productsapi/EndVisit',
                    function (data: any) {
                      (window as { [key: string]: any })['PlaceMessage'](data);
                    }
                  );
                }}
              >
                End Visit
              </Button>
            </div>
          </div>
        </div>
      </>
    );
  }
};
export default ProductSessionButtons;
