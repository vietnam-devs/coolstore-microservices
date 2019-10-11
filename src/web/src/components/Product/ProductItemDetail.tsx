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
                    <StyledImg src="/eco-product-img.png" alt="" className="img-fluid" />
                  </div>
                  <div className="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12 pl-xl-0 pl-lg-0 pl-md-0 m-b-30">
                    <div className="product-details">
                      <div className="border-bottom pb-3 mb-3">
                        <h2 className="mb-3">T-Shirt Product Title</h2>
                        <div className="product-rating d-inline-block float-right">
                          <FontAwesomeIcon icon={faStar} color="#ffa811" />
                          <FontAwesomeIcon icon={faStar} color="#ffa811" />
                          <FontAwesomeIcon icon={faStar} color="#ffa811" />
                          <FontAwesomeIcon icon={faStar} color="#ffa811" />
                          <FontAwesomeIcon icon={faStar} color="#ffa811" />
                        </div>
                        <h3 className="mb-0 text-primary">$49.00</h3>
                      </div>
                      <div className="product-size border-bottom">
                        <div className="product-qty">
                          <h4>Quantity</h4>
                          <div className="quantity">
                            <input type="number" min="1" max="9" step="1" value="1" />
                          </div>
                        </div>
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
                    </div>
                  </div>
                </div>
                <div className="row">
                  <div className="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 m-b-10">
                    <h3> Related Products</h3>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
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
