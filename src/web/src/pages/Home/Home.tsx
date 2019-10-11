import React from 'react'
import { Alert } from 'reactstrap'

import { ProductItem, Pagination, Filter } from 'components/Product'

const Home: React.FC = () => {
  return (
    <div className="App">
      <Alert type="primary" hasExtraSpace>
        <strong>Work in progress!</strong> More detailed documentation is coming soon.
      </Alert>

      <div className="dashboard-wrapper">
        <div className="dashboard-ecommerce">
          <div className="container-fluid dashboard-content">
            <div className="row">
              <div className="col-xl-3 col-lg-4 col-md-4 col-sm-12 col-12">
                <Filter></Filter>
              </div>

              <div className="col-xl-9 col-lg-8 col-md-8 col-sm-12 col-12">
                <div className="row">
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-4 col-lg-6 col-md-12 col-sm-12 col-12">
                    <ProductItem></ProductItem>
                  </div>
                  <div className="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <Pagination></Pagination>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Home
