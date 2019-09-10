import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'
import { ApolloProvider } from 'react-apollo-hooks'

import './index.css'
import App from './App'
import client from './apollo-client'

import * as serviceWorker from './serviceWorker'

client().then(c => {
  const initData = () =>
    c.writeData({
      data: {
        productSearch: {
          __typename: 'ProductSearch',
          price: 999,
          page: 1
        }
      }
    })

  initData()

  c.onResetStore(async () => {
    initData()
  })
  c.onClearStore(async () => {
    initData()
  })

  const rootElement = document.getElementById('root')

  ReactDOM.render(
    <ApolloProvider client={c}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </ApolloProvider>,
    rootElement
  )

  // If you want your app to work offline and load faster, you can change
  // unregister() to register() below. Note this comes with some pitfalls.
  // Learn more about service workers: http://bit.ly/CRA-PWA
  serviceWorker.unregister()
})
