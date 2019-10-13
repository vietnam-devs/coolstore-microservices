import React, { useState, useEffect } from 'react'
import { Container, Row, Col, Media, Table, Input, Button } from 'reactstrap'
import styled from 'styled-components'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons'
import axios from 'axios'

import { Header, Footer } from 'components/App'

const StyledProductImg = styled.img`
  margin-right: 10px;
`

const StyledContainer = styled.div`
  margin: 15px 0;
`

const Cart: React.FC = () => {
  return (
    <>
      <Header></Header>

      <StyledContainer>
        <Container>
          <Row>
            <Col sm="8">
              <Table size="sm">
                <thead>
                  <tr>
                    <th>#</th>
                    <th>Product</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <th scope="row">1</th>
                    <td>
                      <Media>
                        <Media left href="#">
                          <StyledProductImg src="/eco-product-img.png" width="64" height="64"></StyledProductImg>
                        </Media>
                        <Media body>
                          <Media heading>Product name</Media>
                          <p>Product desc...</p>
                        </Media>
                      </Media>
                    </td>
                    <td>
                      <div className="text-muted">$12</div>
                    </td>
                    <td>
                      <Input
                        type="number"
                        name="number"
                        id="quantity"
                        defaultValue="1"
                        placeholder="choose product quantity"
                      />
                    </td>
                    <td>
                      <Button color="danger" size="sm">
                        <FontAwesomeIcon icon={faTrashAlt} />
                      </Button>
                    </td>
                  </tr>
                  <tr>
                    <th scope="row">2</th>
                    <td>
                      <Media>
                        <Media left href="#">
                          <StyledProductImg src="/eco-product-img.png" width="64" height="64"></StyledProductImg>
                        </Media>
                        <Media body>
                          <Media heading>Product name</Media>
                          <p>Product desc...</p>
                        </Media>
                      </Media>
                    </td>
                    <td>
                      <div className="text-muted">$12</div>
                    </td>
                    <td>
                      <Input
                        type="number"
                        name="number"
                        id="quantity"
                        defaultValue="1"
                        placeholder="choose product quantity"
                      />
                    </td>
                    <td>
                      <Button color="danger" size="sm">
                        <FontAwesomeIcon icon={faTrashAlt} />
                      </Button>
                    </td>
                  </tr>
                </tbody>
              </Table>
            </Col>
            <Col sm="4">
              <div className="card">
                <div className="card-header">
                  <h4 className="d-flex align-items-center mb-0">
                    <span className="text-muted">Summary</span>
                  </h4>
                </div>
                <ul className="list-group list-group-flush">
                  <li className="list-group-item">Cart total: $31,186</li>
                  <li className="list-group-item">Promotional item saving: $0</li>
                  <li className="list-group-item">Subtotal: $0</li>
                  <li className="list-group-item">Shipping: $0</li>
                  <li className="list-group-item">Promotional shipping saving: $0</li>
                  <li className="list-group-item">Total order mount: $31,186</li>
                </ul>
                <div className="card-footer">
                  <Button color="primary" className="btn-block">
                    Checkout
                  </Button>
                </div>
              </div>
            </Col>
          </Row>
        </Container>
      </StyledContainer>

      <Footer></Footer>
    </>
  )
}

export default Cart
