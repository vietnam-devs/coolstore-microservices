import React, { useEffect, useCallback } from 'react'
import { Container, Row, Col } from 'reactstrap'
import styled from 'styled-components'

import { AppActions, useStore } from 'stores/store'
import { getCartForCurrentUser, deleteCartForCurrentUser } from 'services/CartService'

import { CartItems, CartSummary } from 'components/Cart'
import { withLayout } from 'components/HOC'

const StyledContainer = styled.div`
  margin: 15px 0;
`

const Cart: React.FC = () => {
  const { state, dispatch } = useStore()

  const fetchData = useCallback(async () => {
    const cart = await getCartForCurrentUser()
    dispatch(AppActions.loadCart(cart))
  }, [dispatch])

  const onProductDeleted = async (cartId: string, productId: string) => {
    const deletedProductId = await deleteCartForCurrentUser(cartId, productId)
    dispatch(AppActions.deleteProductInCart(deletedProductId))
  }

  useEffect(() => {
    fetchData()
  }, [state.isCartLoaded, fetchData])

  return (
    <>
      {state.isCartLoaded && state.cart && state.cart.items.length > 0 && (
        <StyledContainer>
          <Container fluid>
            <Row>
              <Col sm="8">
                <CartItems cart={state.cart} onProductDeleted={onProductDeleted}></CartItems>
              </Col>
              <Col sm="4">
                <CartSummary cart={state.cart}></CartSummary>
              </Col>
            </Row>
          </Container>
        </StyledContainer>
      )}
    </>
  )
}

export default withLayout(Cart)
