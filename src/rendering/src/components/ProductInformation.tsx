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
    variants: productVariantI[];
  };
};

interface productVariantI {
  id: string;
  name: string;
  specs: specModelI[];
  imageUrl: string;
}

interface specModelI {
  id: string;
  value: string;
}

const ProductInformation = ({ fields }: ProductInformationProps): JSX.Element => {
  React.useEffect(() => {
    (window as { [key: string]: any })['InitializeSelectVariants']();
  }, []);

  const [value, setValue] = React.useState(false);

  return (
    <>
      <div className="container">
        <div className="row">
          <div className="col-12">
            <h1>{fields.name}</h1>
          </div>
          <div className="col-4">
            <img id="productImage" src={fields?.image ?? ''} width="100%" height="auto" />
          </div>
          <div className="col-7">
            <h3>Basic Information</h3>
            <p>
              ID: <b> {fields.id}</b>
            </p>
            <div>
              <div dangerouslySetInnerHTML={{ __html: fields.description }} />
            </div>
            <div>
              {fields.variants.length !== 0 ? (
                <select id="variants">
                  {fields.variants.map((variant) => (
                    <option key={variant.id} value={variant.id}>
                      {variant.name == '' ? variant.id : variant.name}
                    </option>
                  ))}
                </select>
              ) : null}
              <hr />
              {fields.variants.length !== 0 ? (
                <div id="variant-data">
                  {fields.variants.map((variant) => (
                    <div id={variant.id} key={variant.id} img-url={variant.imageUrl}></div>
                  ))}
                </div>
              ) : null}
            </div>
            <div className="col-6">
              <div className="SitecoreButton" onClick={() => setValue(!value)}>
                <a>More options</a>
              </div>
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
