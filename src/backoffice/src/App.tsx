import React, { Component } from 'react'
import { Route, Redirect } from 'react-router'

import { AuthService } from './services/AuthService'

import Layout from './components/Layout'
import Home from './routes/Home'
import ProductList from './routes/ProductList'

class App extends Component<any, any> {
  public authService: AuthService
  constructor(props: any) {
    super(props)
    this.authService = new AuthService()
  }

  componentDidMount() {
    this.authService.getUser().then(user => {
      if (user != null && user.access_token != null) {
        this.setState({
          token: user.access_token
        })
      }
    })
  }

  render() {
    console.log(this.state)
    return (
      <div className="App">
        <Layout>
          <Route path="/" exact component={Home} />
          <Route
            path="/products"
            render={props =>
              this.state != null && this.state.token != null ? (
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
}

export default App
