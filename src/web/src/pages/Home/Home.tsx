import React, { useEffect, useCallback } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { ProductItem, Pagination, Filter } from 'components/Product'
import { withLayout } from 'components/HOC'

import { AppActions, useStore } from 'stores/store'
import { getProducts } from 'services/ProductService'
import { getCartForCurrentUser, createCartForCurrentUser, updateCartForCurrentUser } from 'services/CartService'

interface IProps extends RouteComponentProps {}

const Home: React.FC<IProps> = props => {
  const { state, dispatch } = useStore()

  const fetchData = useCallback(
    async (page: number, price: number) => {
      const products = await getProducts(page, price)
      dispatch(AppActions.loadProducts(products))
    },
    [dispatch]
  )

  const onPriceFilterChange = (price: number) => {
    fetchData(1, price)
  }

  const onAddProductToCart = async (productId: string) => {
    // get and dispatch to cart store
    let cart = await getCartForCurrentUser()

    dispatch(AppActions.loadCart(cart))

    // check if add or update need to be happened
    if (cart == null) {
      cart = await createCartForCurrentUser(productId)
    } else {
      cart = await updateCartForCurrentUser(cart.id, productId, 1)
    }

    // dispatch and notification
    dispatch(AppActions.updateProductInCart({ productId: productId, quantity: 1 }))
    dispatch(AppActions.showNotification(`One item has already added to the cart.`))
  }

  useEffect(() => {
    fetchData(1, 888)
  }, [state.isProductsLoaded, fetchData])

  return (
    <>
      {state.products.length > 0 && (
        <div className="container-fluid">
          <div className="row">
            <div className="col-xl-2 col-lg-4 col-md-4 col-sm-12 col-12">
              <Filter onPriceFilterChange={onPriceFilterChange} initPrice={888} maxPrice={10000}></Filter>
            </div>

            <div className="col-xl-10 col-lg-8 col-md-8 col-sm-12 col-12">
              <div className="row">
                {state.products.map(item => (
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12" key={item.id}>
                    <ProductItem {...props} data={item} onAddProductToCart={onAddProductToCart}></ProductItem>
                  </div>
                ))}

                <div className="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                  <Pagination></Pagination>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  )
}

export default withLayout(Home)
