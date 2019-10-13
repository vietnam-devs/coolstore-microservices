import React from 'react'
import { withRouter } from 'react-router-dom'
import { Switch, Route } from 'react-router-dom'

import requireAuth from 'components/HOC/requireAuth'
import Home from 'pages/Home'
import ProductDetail from 'pages/ProductDetail'
import { Cart } from 'pages/Cart'
import { Callback, SilentCallback, NotAuth } from 'pages/Authentication'
import NotFound from 'pages/404'

const AppLayout = ({ location }: any) => {
  return (
    <>
      <Switch>
        <Route exact path={'/'} component={requireAuth(Home)} />
        <Route path={'/product/:id'} component={requireAuth(ProductDetail)} />
        <Route path={'/cart'} component={requireAuth(Cart)} />
        <Route path={'/auth/callback'} component={Callback} />
        <Route path={'/auth/silent-renew'} component={SilentCallback} />
        <Route path={'/401'} component={NotAuth} />
        <Route component={NotFound} />
      </Switch>
    </>
  )
}

export default withRouter(AppLayout)
