import React from 'react'
import styled from 'styled-components'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons'

import { ProductItem } from 'components/Product'

const StyledImg = styled.img`
  width: 100%;
  padding: 0.5rem;
  height: 370px;
`

const StyledProductDetails = styled.div`
  background-color: #fff;
  padding: 30px;
  border-top-right-radius: 4px;
  border-bottom-right-radius: 4px;
  position: relative;
`

const StyledProductRating = styled.div`
  font-size: 12px;
  color: #ffa811 !important;
`

const StyledProductQuantity = styled.div`
  position: absolute;
  right: 0;
  top: 0px;
  &input[type='number']::-webkit-inner-spin-button,
  &input[type='number']::-webkit-outer-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
  &input[type='number'] {
    -moz-appearance: textfield;
  }
`

const StyledQuantity = styled.div`
  position: relative;
  &input {
    width: 65px;
    height: 41px;
    line-height: 1.65;
    float: left;
    display: block;
    padding: 0;
    margin: 0;
    padding-left: 20px;
    border: 1px solid #eee;
    &:focus {
      outline: 0;
    }
  }
`

const ProductItemDetail: React.FC = () => {
  return (
    <>
      <div className="dashboard-wrapper">
        <div className="dashboard-ecommerce">
          <div className="container-fluid dashboard-content">
            <div className="row">
              <div className="offset-xl-2 col-xl-8 col-lg-12 col-md-12 col-sm-12 col-12">
                <div className="row">
                  <div className="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12 pr-xl-0 pr-lg-0 pr-md-0  m-b-30">
                    <StyledImg src="https://picsum.photos/1200/900?image=20" alt="" className="img-fluid" />
                  </div>
                  <div className="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12 pl-xl-0 pl-lg-0 pl-md-0 m-b-30">
                    <StyledProductDetails>
                      <div className="border-bottom pb-3 mb-3">
                        <h2 className="mb-3">T-Shirt Product Title</h2>
                        <StyledProductRating className="d-inline-block float-right">
                          <FontAwesomeIcon icon={faStar} />
                          <FontAwesomeIcon icon={faStar} />
                          <FontAwesomeIcon icon={faStar} />
                          <FontAwesomeIcon icon={faStar} />
                          <FontAwesomeIcon icon={faStar} />
                        </StyledProductRating>
                        <h3 className="mb-0 text-primary">$49.00</h3>
                      </div>
                      <div className="product-size border-bottom">
                        <StyledProductQuantity>
                          <h4>Quantity</h4>
                          <StyledQuantity>
                            <input type="number" min="1" max="9" step="1" value="1" />
                          </StyledQuantity>
                        </StyledProductQuantity>
                      </div>
                      <div className="product-description">
                        <h4 className="mb-1">Descriptions</h4>
                        <p>
                          Praesent et cursus quam. Etiam vulputate est et metus pellentesque iaculis. Suspendisse nec
                          urna augue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia
                          Curae;
                        </p>
                        <a href="#" className="btn btn-primary btn-block btn-lg">
                          Add to Cart
                        </a>
                      </div>
                    </StyledProductDetails>
                  </div>
                </div>
                <div className="row">
                  <div className="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 m-b-10">
                    <h3> Related Products</h3>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    {/* <ProductItem></ProductItem> */}
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    {/* <ProductItem></ProductItem> */}
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    {/* <ProductItem></ProductItem> */}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  )
}

export default ProductItemDetail
