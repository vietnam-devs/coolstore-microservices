import React from 'react'
import { Link } from 'react-router-dom'
import { Button } from 'reactstrap'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar, faHeart, faShoppingCart, faShareSquare } from '@fortawesome/free-solid-svg-icons'
import styled from 'styled-components'

import { IProduct } from 'stores/types'

const StyledImg = styled.img`
  width: 100%;
`

const StyledProductThumbnail = styled.div`
  border: 1px solid #e6e6f2;
  background-color: #fff;
  margin: 0.3rem 0;
`

const StyledProductImgHead = styled.div`
  position: relative;
`

const StyledProductImg = styled.div`
  text-align: center;
  padding: 35px 0px;
`

const StyledRibbons = styled.div`
  -webkit-clip-path: polygon(
    10% 25%,
    10% 0,
    35% 0%,
    65% 0%,
    90% 0,
    90% 25%,
    90% 50%,
    91% 100%,
    50% 73%,
    10% 100%,
    10% 50%
  );
  clip-path: polygon(10% 25%, 10% 0, 35% 0%, 65% 0%, 90% 0, 90% 25%, 90% 50%, 91% 100%, 50% 73%, 10% 100%, 10% 50%);

  position: absolute;
  top: 0px;
  background-color: #59b3ff;
  padding: 31px 15px;
  text-align: center;
  left: 10px;
  font-family: 'Circular Std Medium';
  color: #fff;
`

const StyledRibbonsText = styled.div`
  transform: rotate(90deg);
  position: absolute;
  top: 11px;
  left: 10px;
  color: #fff;
`

const StyledProductWishListBtn = styled.a`
  height: 40px;
  width: 40px;
  border: 2px solid #dfdfec;
  border-radius: 100px;
  font-size: 18px;
  line-height: 2.3;
  color: #dfdfec;
  text-align: center;
  display: block;
  position: absolute;
  right: 15px;
  top: 15px;

  &:hover {
    border-color: #ff3367;
    color: #ff3367;
    transition: 0.3s ease;
  }

  &.active {
    background-color: #ff3367;
    color: #ffffff;
    transition: 0.3s ease;
  }
`

const StyledProductContent = styled.div`
  border-top: 1px solid #e6e6f2;
  padding: 23px;
`

const StyledProductContentHead = styled.div`
  position: relative;
  margin-bottom: 25px;
`

const StyledProductTitle = styled.h3`
  font-size: 16px;
  margin-bottom: 5px;
`

const StyledProductRating = styled.div`
  font-size: 12px;
  color: #ffa811 !important;
`

const StyledProductPrice = styled.div`
  position: absolute;
  top: 0;
  right: 0;
  font-size: 16px;
  color: #3d405c;
  font-family: 'Circular Std Medium';
  line-height: 1;
`

const StyledDetailButton = styled(Link)`
  margin: 0 0 0 5px;
`

interface IProps {
  data: IProduct
  onAddProductToCart: (productId: string) => void
}

const ProductItem: React.FC<IProps> = ({ data, onAddProductToCart }) => {
  return (
    <>
      <StyledProductThumbnail>
        <StyledProductImgHead>
          <StyledProductImg>
            <Link to={`/product/${data.id}`}>
              <StyledImg src={data.imageUrl} alt="" className="img-fluid" />
            </Link>
          </StyledProductImg>

          <StyledRibbons className="bg-brand"></StyledRibbons>

          <StyledRibbonsText>Offer</StyledRibbonsText>

          <StyledProductWishListBtn href="#">
            <FontAwesomeIcon icon={faHeart} />
          </StyledProductWishListBtn>
        </StyledProductImgHead>

        <StyledProductContent>
          <StyledProductContentHead>
            <StyledProductTitle className="product-title">
              {data.name.replace(/^(.{25}[^\s]*).*/, '$1')}
            </StyledProductTitle>

            <StyledProductRating className="d-inline-block">
              <FontAwesomeIcon icon={faStar} />
              <FontAwesomeIcon icon={faStar} />
              <FontAwesomeIcon icon={faStar} />
              <FontAwesomeIcon icon={faStar} />
              <FontAwesomeIcon icon={faStar} />
            </StyledProductRating>

            <StyledProductPrice>${data.price}</StyledProductPrice>
          </StyledProductContentHead>

          <Button
            color="primary"
            onClick={() => {
              onAddProductToCart(data.id)
            }}
          >
            <FontAwesomeIcon icon={faShoppingCart}></FontAwesomeIcon> Add to Cart
          </Button>

          <StyledDetailButton className="btn btn-secondary" to={`/product/${data.id}`}>
            <FontAwesomeIcon icon={faShareSquare}></FontAwesomeIcon> Detail
          </StyledDetailButton>
        </StyledProductContent>
      </StyledProductThumbnail>
    </>
  )
}

export default ProductItem
