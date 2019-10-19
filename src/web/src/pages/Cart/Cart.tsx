import React, { useState, useEffect, useCallback } from 'react'
import { Container, Row, Col } from 'reactstrap'
import styled from 'styled-components'

import { AppActions, useStore } from 'stores/store'
import { getCartForCurrentUser, updateCartForCurrentUser, deleteCartForCurrentUser } from 'services/CartService'
import { ICart } from 'stores/types'
import { CartItems, CartSummary } from 'components/Cart'
import { withLayout } from 'components/HOC'

const StyledContainer = styled.div`
  margin: 15px 0;
`

const Cart: React.FC = () => {
  const { state, dispatch } = useStore()
  const [cart, setCart] = useState<ICart>(null)

  const fetchData = useCallback(async () => {
    let cart = await getCartForCurrentUser()
    setCart(cart)
    dispatch(AppActions.loadCart(cart))
  }, [dispatch])

  const onProductDeleted = async (cartId: string, productId: string) => {
    const deletedProductId = await deleteCartForCurrentUser(cartId, productId)
    dispatch(AppActions.deleteProductInCart(deletedProductId))
  }

  const onProductUpdated = async (productId: string, quantity: number) => {
    if (cart) {
      let updatedCart = await updateCartForCurrentUser(cart.id, productId, quantity)
      setCart(updatedCart)
      dispatch(AppActions.updateProductInCart({ productId: productId, quantity: quantity }))
    }
  }

  useEffect(() => {
    fetchData()
  }, [state.isCartLoaded, fetchData])

  return (
    <>
      {state.isCartLoaded && cart && cart.items.length > 0 && (
        <StyledContainer>
          <Container fluid>
            <Row>
              <Col sm="8">
                {cart && (
                  <CartItems
                    cart={cart}
                    onProductUpdated={onProductUpdated}
                    onProductDeleted={onProductDeleted}
                  ></CartItems>
                )}
              </Col>
              <Col sm="4">{cart && <CartSummary cart={cart}></CartSummary>}</Col>
            </Row>
          </Container>
        </StyledContainer>
      )}
    </>
  )
}

export default withLayout(Cart)
