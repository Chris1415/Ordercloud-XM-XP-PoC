import { Text, Field, ImageField, Image } from '@sitecore-jss/sitecore-jss-nextjs';
import { StyleguideComponentProps } from 'lib/component-props';
import * as React from 'react';
import AjaxResponseContainer from './AjaxResponseContainer';
import ProductInteractionButtons from './ProductInteractionButtons';

export type ProductTeaserProps = StyleguideComponentProps & {
  fields: {
    Id: Field<string>;
    Name: Field<string>;
    Image: ImageField;
    Description: Field<string>;
  };
};

const ProductTeaser = ({ fields }: ProductTeaserProps): JSX.Element => {
  return (
    <div className="container">
      <div className="row">
        <div className="col-12">
          <h1>
            <Text field={fields.Name} />
          </h1>
        </div>
        <div className="col-4">
          <Image
            className="img-responsive"
            media={fields.Image}
            imageParams={{ mw: 200 }}
            width="100%"
            height="auto"
          />
        </div>
        <div className="col-7">
          <h3>Basic Information</h3>
          <p>
            ID:{' '}
            <b>
              {' '}
              <Text field={fields.Id} />{' '}
            </b>
          </p>
          <div>
            <Text encode={false} field={fields.Description} />
          </div>
        </div>
      </div>
      <hr />
      {ProductInteractionButtons(fields.Id.value)}
      {AjaxResponseContainer}
    </div>
  );
};

export default ProductTeaser;
