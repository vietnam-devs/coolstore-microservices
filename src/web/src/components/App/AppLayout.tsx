import React from 'react'
import { withRouter } from 'react-router-dom'
import { Switch, Route } from 'react-router-dom'

import Home from 'pages/Home'

const AppLayout = ({ location }: any) => {
  return (
    <>
      <Switch>
        <Route exact path={'/'} component={Home} />
      </Switch>
    </>
  )
}

export default withRouter(AppLayout)
