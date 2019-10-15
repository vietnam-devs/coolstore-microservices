import React from 'react'
import { Button } from 'reactstrap'

import { ICart } from 'stores/types'

interface IProps {
  cart: ICart
}

const CartSummary: React.FC<IProps> = ({ cart }) => {
  return (
    <div className="card">
      <div className="card-header">
        <h4 className="d-flex align-items-center mb-0">
          <span className="text-muted">Summary</span>
        </h4>
      </div>

      <ul className="list-group list-group-flush">
        <li className="list-group-item">Cart total: ${cart.cartTotal.toFixed(2)}</li>
        <li className="list-group-item">Promotional item saving: ${cart.cartItemPromoSavings.toFixed(2)}</li>
        <li className="list-group-item">Subtotal: ${cart.cartItemTotal.toFixed(2)}</li>
        <li className="list-group-item">Shipping: ${cart.shippingTotal.toFixed(2)}</li>
        <li className="list-group-item">Promotional shipping saving: ${cart.shippingPromoSavings.toFixed(2)}</li>
        <li className="list-group-item">
          Total order mount: ${(cart.cartTotal + cart.cartItemTotal + cart.shippingTotal).toFixed(2)}
        </li>
      </ul>

      <div className="card-footer">
        <Button color="primary" className="btn-block">
          Checkout
        </Button>
      </div>
    </div>
  )
}

export default CartSummary
