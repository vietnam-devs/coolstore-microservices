import React from 'react'
import { withRouter } from 'react-router-dom'
import { Switch, Route } from 'react-router-dom'

import requireAuth from 'components/HOC/requireAuth'

import Home from 'pages/Home'
import Callback from 'pages/Callback/Callback'
import SilentCallback from 'pages/Callback/SilentCallback'

const AppLayout = ({ location }: any) => {
  return (
    <>
      <Switch>
        <Route exact path={'/'} component={requireAuth(Home)} />
        <Route exact path={'/auth/callback'} component={Callback} />
        <Route exact path={'/auth/silent-callback'} component={SilentCallback} />
      </Switch>
    </>
  )
}

export default withRouter(AppLayout)
