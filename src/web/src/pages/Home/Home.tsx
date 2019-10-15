import React, { useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'
import { Alert } from 'reactstrap'

import { Header, Footer } from 'components/App'
import { ProductItem, Pagination, Filter } from 'components/Product'

import { AppActions, useStore } from 'stores/store'
import { getProducts } from 'services/ProductService'

interface IProps extends RouteComponentProps {}

const Home: React.FC<IProps> = props => {
  const { state, dispatch } = useStore()

  async function fetchData(page: number, price: number) {
    const products = await getProducts(page, price)
    dispatch(AppActions.loadProducts(products))
  }

  const onPriceFilterChange = (e: React.MouseEvent<HTMLInputElement, MouseEvent>) => {
    fetchData(1, +e.currentTarget.value)
  }

  useEffect(() => {
    fetchData(1, 500)
  }, [state.isProductsLoaded])

  return (
    <>
      <Header></Header>

      <div className="App">
        <Alert color="warning">
          <strong>Work in progress!</strong> More changes are coming soon.
        </Alert>

        {state.products.length > 0 && (
          <div className="dashboard-wrapper">
            <div className="dashboard-ecommerce">
              <div className="container-fluid dashboard-content">
                <div className="row">
                  <div className="col-xl-2 col-lg-4 col-md-4 col-sm-12 col-12">
                    <Filter onPriceFilterChange={onPriceFilterChange}></Filter>
                  </div>

                  <div className="col-xl-10 col-lg-8 col-md-8 col-sm-12 col-12">
                    <div className="row">
                      {state.products.map(item => (
                        <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12" key={item.id}>
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
