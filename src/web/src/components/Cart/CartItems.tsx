import React from 'react'
import { Link } from 'react-router-dom'
import { Media, Table, Input, Button } from 'reactstrap'
import styled from 'styled-components'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTrashAlt } from '@fortawesome/free-solid-svg-icons'

import { ICart } from 'stores/types'

const StyledProductImg = styled.img`
  margin-right: 10px;
`

interface IProps {
  cart: ICart
  onProductDeleted: (cartId: string, productId: string) => void
  onProductUpdated: (productId: string, quantity: number) => void
}

const CartItems: React.FC<IProps> = ({ cart, onProductUpdated, onProductDeleted }) => {
  return (
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
        {cart.items.map((item, index) => (
          <tr key={item.productId}>
            <th scope="row">{index + 1}</th>
            <td>
              <Media>
                <Media left>
                  <StyledProductImg src={item.productImagePath} width="64" height="64"></StyledProductImg>
                </Media>
                <Media body>
                  <Media heading>
                    <Link to={`/product/${item.productId}`}>{item.productName.replace(/^(.{25}[^\s]*).*/, '$1')}</Link>
                  </Media>
                  <p>{item.productDesc.replace(/^(.{50}[^\s]*).*/, '$1')}</p>
                </Media>
              </Media>
            </td>
            <td>
              <div className="text-muted">${item.productPrice}</div>
            </td>
            <td>
              <Input
                type="number"
                name="number"
                id="quantity"
                defaultValue={item.quantity}
                min={1}
                placeholder="choose product quantity"
                onClick={() => onProductUpdated(item.productId, 1)}
              />
            </td>
            <td>
              <Button color="danger" size="sm" onClick={() => onProductDeleted(cart.id, item.productId)}>
                <FontAwesomeIcon icon={faTrashAlt} />
              </Button>
            </td>
          </tr>
        ))}
      </tbody>
    </Table>
  )
}

export default CartItems
