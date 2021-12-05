// import { Text, Field, ImageField, Image } from '@sitecore-jss/sitecore-jss-nextjs';
import { StyleguideComponentProps } from 'lib/component-props';
import * as React from 'react';
import AjaxResponseContainer from './AjaxResponseContainer';
import ProductInteractionButtons from './ProductInteractionButtons';
import ProductSessionButtons from './ProductSessionButtons';

type ProductInformationProps = StyleguideComponentProps & {
  fields: {
    id: string;
    name: string;
    image: string;
    description: string;
  };
};

const ProductInformation = ({ fields }: ProductInformationProps): JSX.Element => {
  const [value, setValue] = React.useState(false);

  return (
    <>
      <div className="sidebarButton">
        <button onClick={() => setValue(!value)}>More options</button>
      </div>

      <div className="container">
        <div className="row">
          <div className="col-12">
            <h1>{fields.name}</h1>
          </div>
          <div className="col-4">
            <img src={fields?.image ?? ''} width="100%" height="auto" />
          </div>
          <div className="col-7">
            <h3>Basic Information</h3>
            <p>
              ID: <b> {fields.id}</b>
            </p>
            <div>
              <div dangerouslySetInnerHTML={{ __html: fields.description }} />
            </div>
          </div>
        </div>
        <hr />
        <div className="row">
          <div className="col-12">
            {ProductInteractionButtons(fields.id)}
            {ProductSessionButtons(value)}
            {AjaxResponseContainer()}
          </div>
        </div>
      </div>
    </>
  );
};

export default ProductInformation;
