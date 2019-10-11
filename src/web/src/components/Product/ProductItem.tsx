import React from 'react'
import { Button, ButtonGroup } from 'reactstrap'
import styled from 'styled-components'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons'

const StyledImg = styled.img`
  width: 100%;
`

const StyledProductThumbnail = styled.div`
  margin: 0.3rem 0;
`

const ProductItem: React.FC = () => {
  return (
    <>
      <StyledProductThumbnail className="product-thumbnail">
        <div className="product-img-head">
          <div className="product-img">
            <StyledImg src="/eco-product-img.png" alt="" className="img-fluid" />
          </div>
          <div className="ribbons bg-brand"></div>
          <div className="ribbons-text">Offer</div>
          <div className="">
            <a href="#" className="product-wishlist-btn active">
              <i className="fas fa-heart"></i>
            </a>
          </div>
        </div>
        <div className="product-content">
          <div className="product-content-head">
            <h3 className="product-title">T-Shirt Product Title</h3>
            <div className="product-rating d-inline-block">
              <FontAwesomeIcon icon={faStar} color="#ffa811" />
              <FontAwesomeIcon icon={faStar} color="#ffa811" />
              <FontAwesomeIcon icon={faStar} color="#ffa811" />
              <FontAwesomeIcon icon={faStar} color="#ffa811" />
              <FontAwesomeIcon icon={faStar} color="#ffa811" />
            </div>
            <div className="product-price">
              $49.00
              <del className="product-del">$69.00</del>
            </div>
          </div>
          <div className="product-btn">
            <ButtonGroup>
              <Button color="primary">Add to Cart</Button>
              <a href={`/product/${1}`} className="btn btn-secondary">
                Details
              </a>
            </ButtonGroup>
          </div>
        </div>
      </StyledProductThumbnail>
    </>
  )
}

export default ProductItem
