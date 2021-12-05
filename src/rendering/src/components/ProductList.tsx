import { Text, Image } from '@sitecore-jss/sitecore-jss-nextjs';
import { StyleguideComponentProps } from 'lib/component-props';
import * as React from 'react';
import AjaxResponseContainer from './AjaxResponseContainer';
import ProductInteractionButtons from './ProductInteractionButtons';
import { ProductTeaserProps } from './ProductTeaser';

type ProductListProps = StyleguideComponentProps & {
  fields: {
    Products: ProductTeaserProps[];
  };
};

const ProductList = ({ fields }: ProductListProps): JSX.Element => {
  return (
    <div className="container product_list">
      <div className="row">
        {fields?.Products?.length == 0 ? (
          <div>No Products</div>
        ) : (
          fields.Products.map((product) => {
            return (
              // eslint-disable-next-line react/jsx-key
              <div className="col-4 product_list__teaser">
                <div className="container">
                  <div className="col-12">
                    <h1>
                      <Text field={product.fields.Name} />
                    </h1>
                  </div>
                  <div className="col-12">
                    <Image
                      className="img-responsive"
                      media={product.fields.Image}
                      imageParams={{ mw: 200 }}
                      width="100%"
                      height="auto"
                    />
                  </div>
                  <div className="col-12">
                    <h3>Basic Information</h3>
                    <p>
                      ID:{' '}
                      <b>
                        {' '}
                        <Text field={product.fields.Id} />{' '}
                      </b>
                    </p>
                    <div>
                      <Text encode={false} field={product.fields.Description} />
                    </div>
                  </div>
                </div>
                <hr />
                {ProductInteractionButtons(product.fields.Id.value)}
              </div>
            );
          })
        )}
      </div>
      {AjaxResponseContainer()}
    </div>
  );
};

export default ProductList;
