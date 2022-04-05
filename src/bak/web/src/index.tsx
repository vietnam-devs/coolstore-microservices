import React from 'react'
import ReactDOM from 'react-dom'
import { ThemeProvider } from 'styled-components'

import 'bootstrap/dist/css/bootstrap.min.css'
import { App } from './components/App'
import { AppProvider } from 'stores'
import * as serviceWorker from './serviceWorker'

var theme = {}

ReactDOM.render(
  <ThemeProvider theme={theme}>
    <AppProvider>
      <App />
    </AppProvider>
  </ThemeProvider>,
  document.getElementById('root')
)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister()
