import React, { Component } from 'react'
import { Route } from 'react-router'
import './App.css'
import Layout from './components/Layout'
import Index from './pages/index'
import Another from './pages/Another'

class App extends Component {
  render() {
    return (
      <div className="App">
        <Layout>
          <Route path="/" exact component={Index} />
          <Route path="/another" component={Another} />
        </Layout>
      </div>
    )
  }
}

export default App
