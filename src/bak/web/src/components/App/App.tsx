import React from 'react'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'

import AppLayout from './AppLayout'

const App: React.FC = () => {
  return (
    <>
      <Router>
        <Switch>
          <Route exact component={AppLayout} />
        </Switch>
      </Router>
    </>
  )
}

export default App
