import React from 'react'
import { Route, Redirect } from 'react-router'
import { useAsync } from 'react-use'

import { AuthService } from './services/AuthService'

import Layout from './components/Layout'
import Home from './routes/Home'
import ProductList from './routes/ProductList'

const getUser = () => new AuthService().getUser()

export default () => {
  const state: any = useAsync(getUser)
  return (
    <div className="App">
      <Layout>
        <Route path="/" exact component={Home} />
        <Route
          path="/products"
          render={() =>
            !state.loading && state.value != null && state.value.access_token != null ? (
              <ProductList />
            ) : (
              <Redirect
                to={{
                  pathname: '/'
                }}
              />
            )
          }
        />
      </Layout>
    </div>
  )
}
