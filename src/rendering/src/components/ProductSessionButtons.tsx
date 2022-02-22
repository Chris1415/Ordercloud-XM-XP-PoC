import * as React from 'react';

const ProductSessionButtons = (render: boolean): JSX.Element => {
  if (!render) {
    return <></>;
  } else {
    return (
      <>
        <hr />
        <div className="container">
          <div className="row">
            <div className="col-12">
              <h3>Test Buttons for Session/Facet information</h3>
            </div>
            <div className="col-3">
              <div
                className="SitecoreButton SpaceSRight"
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
              </div>
            </div>
            <div className="col-3">
              <div
                className="SitecoreButton SpaceSRight"
                onClick={() => {
                  (window as { [key: string]: any })['Ajax'](
                    'https://cm.jss18demo.localhost/api/sitecore/productsapi/CreateOrUpdateContact',
                    function (data: any) {
                      (window as { [key: string]: any })['PlaceMessage'](data);
                    }
                  );
                }}
              >
                Create or Update Contact in SC
              </div>
            </div>
            <div className="col-3">
              <div
                className="SitecoreButton SpaceSRight"
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
              </div>
            </div>
            <div className="col-3">
              <div
                className="SitecoreButton SpaceSRight"
                onClick={() => {
                  (window as { [key: string]: any })['Ajax'](
                    'https://cm.jss18demo.localhost/api/sitecore/productsapi/UpdateOrdercloudFacets',
                    function (data: any) {
                      (window as { [key: string]: any })['PlaceMessage'](data);
                    }
                  );
                }}
              >
                Update Contact in Ordercloud
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }
};
export default ProductSessionButtons;
