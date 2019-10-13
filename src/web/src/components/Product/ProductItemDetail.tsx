import React from 'react'
import { Input, Label } from 'reactstrap'
import styled from 'styled-components'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons'

import { IProduct } from 'stores/store'

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

interface IProps {
  data: IProduct
}

const ProductItemDetail: React.FC<IProps> = ({ data }) => {
  return (
    <>
      {data && (
        <div className="dashboard-wrapper">
          <div className="dashboard-ecommerce">
            <div className="container-fluid dashboard-content">
              <div className="row">
                <div className="offset-xl-2 col-xl-8 col-lg-12 col-md-12 col-sm-12 col-12">
                  <div className="row">
                    <div className="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12 pr-xl-0 pr-lg-0 pr-md-0  m-b-30">
                      <StyledImg src={data.imageUrl} alt="" className="img-fluid" />
                    </div>
                    <div className="col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12 pl-xl-0 pl-lg-0 pl-md-0 m-b-30">
                      <StyledProductDetails>
                        <div className="border-bottom pb-3 mb-3">
                          <h2 className="mb-3">{data.name}</h2>
                          <StyledProductRating className="d-inline-block float-right">
                            <FontAwesomeIcon icon={faStar} />
                            <FontAwesomeIcon icon={faStar} />
                            <FontAwesomeIcon icon={faStar} />
                            <FontAwesomeIcon icon={faStar} />
                            <FontAwesomeIcon icon={faStar} />
                          </StyledProductRating>
                          <h3 className="mb-0 text-primary">${data.price}</h3>
                        </div>
                        <div className="product-size border-bottom">
                          <Label for="quantity">
                            <h4 className="mb-1">Quantity</h4>
                          </Label>
                          <Input
                            type="number"
                            name="number"
                            id="quantity"
                            defaultValue="1"
                            placeholder="choose product quantity"
                          />
                        </div>
                        <div className="product-description">
                          <h4 className="mb-1">Descriptions</h4>
                          <p>{data.desc}</p>
                          <a href="#" className="btn btn-primary btn-block btn-lg">
                            Add to Cart
                          </a>
                        </div>
                      </StyledProductDetails>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  )
}

export default ProductItemDetail
