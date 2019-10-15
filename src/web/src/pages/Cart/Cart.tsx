import React, { useEffect, useCallback } from 'react'
import { Container, Row, Col } from 'reactstrap'
import styled from 'styled-components'

import { AppActions, useStore } from 'stores/store'
import { getCartForCurrentUser } from 'services/CartService'

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

  useEffect(() => {
    fetchData()
  }, [state.isCartLoaded, fetchData])

  return (
    <>
      {state.isCartLoaded && state.cart.items.length > 0 && (
        <StyledContainer>
          <Container fluid>
            <Row>
              <Col sm="8">
                <CartItems cart={state.cart}></CartItems>
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
