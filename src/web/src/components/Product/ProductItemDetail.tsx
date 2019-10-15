import React from 'react'
import { Input, Label, Button } from 'reactstrap'
import styled from 'styled-components'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar, faShoppingCart } from '@fortawesome/free-solid-svg-icons'

import { IProduct } from 'stores/types'

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

interface IProps {
  data: IProduct
}

const ProductItemDetail: React.FC<IProps> = ({ data }) => {
  return (
    <>
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
              defaultValue={1}
              min={1}
              placeholder="choose product quantity"
            />
          </div>
          <div className="product-description">
            <h4 className="mb-1">Descriptions</h4>
            <p>{data.desc}</p>
            <Button color="primary" block>
              <FontAwesomeIcon icon={faShoppingCart}></FontAwesomeIcon> Add to Cart
            </Button>
          </div>
        </StyledProductDetails>
      </div>
    </>
  )
}

export default ProductItemDetail
