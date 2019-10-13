import React, { useEffect, useState } from 'react'
import { RouteComponentProps } from 'react-router-dom'
import { Alert } from 'reactstrap'
import axios from 'axios'

import { Header, Footer } from 'components/App'
import { ProductItem, Pagination, Filter } from 'components/Product'
import { AppActions, useStore, IProduct } from 'stores/store'

interface IProps extends RouteComponentProps {}

const Home: React.FC<IProps> = props => {
  const { state, dispatch } = useStore()
  const [products, setProducts] = useState<IProduct[]>([])

  const loadProducts = async (page: number, price: number) => {
    const result = await axios.get(`/api/products/${page}/${price}`, {
      baseURL: `${process.env.REACT_APP_API}`,
      data: {},
      headers: {
        ['Content-Type']: 'application/grpc',
        Authorization: `Bearer ${state.accessToken}`
      }
    })
    setProducts(result.data.products)
    dispatch(AppActions.loadProducts(result.data.products))
  }

  const onPriceFilterChange = (e: React.MouseEvent<HTMLInputElement, MouseEvent>) => {
    if (state.accessToken != null) {
      loadProducts(1, +e.currentTarget.value)
    }
  }

  useEffect(() => {
    if (state.accessToken != null && state.products.length <= 0) {
      loadProducts(1, 500)
    } else {
      setProducts(state.products)
    }
  }, [state])

  return (
    <>
      <Header></Header>

      <div className="App">
        <Alert color="warning">
          <strong>Work in progress!</strong> More changes are coming soon.
        </Alert>

        {products.length > 0 && (
          <div className="dashboard-wrapper">
            <div className="dashboard-ecommerce">
              <div className="container-fluid dashboard-content">
                <div className="row">
                  <div className="col-xl-2 col-lg-4 col-md-4 col-sm-12 col-12">
                    <Filter onPriceFilterChange={onPriceFilterChange}></Filter>
                  </div>

                  <div className="col-xl-10 col-lg-8 col-md-8 col-sm-12 col-12">
                    <div className="row">
                      {products.map(item => (
                        <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                          <ProductItem {...props} data={item}></ProductItem>
                        </div>
                      ))}

                      <div className="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <Pagination></Pagination>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>

      <Footer></Footer>
    </>
  )
}

export default Home
