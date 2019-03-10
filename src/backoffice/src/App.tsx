import React, { Component } from 'react'
import { Route } from 'react-router'

import Layout from './components/Layout'
import Home from './routes/Home'
import ProductList from './routes/ProductList'

class App extends Component {
  render() {
    return (
      <div className="App">
        <Layout>
          <Route path="/" exact component={Home} />
          <Route path="/products" component={ProductList} />
        </Layout>
      </div>
    )
  }
}

export default App
