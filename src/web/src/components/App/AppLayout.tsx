import React, { Suspense, lazy } from 'react'
import { withRouter } from 'react-router-dom'
import { Switch, Route } from 'react-router-dom'

import { withAuth } from 'components/HOC'
import { Callback, SilentCallback, NotAuth } from 'pages/Authentication'
import NotFound from 'pages/404'

const Home = lazy(() => import('pages/Home'))
const ProductDetail = lazy(() => import('pages/ProductDetail'))
const Cart = lazy(() => import('pages/Cart'))

const AppLayout = ({ location }: any) => {
  return (
    <>
      <Suspense fallback={<div>Loading...</div>}>
        <Switch>
          <Route exact path={'/'} component={withAuth(Home)} />
          <Route path={'/product/:id'} component={withAuth(ProductDetail)} />
          <Route path={'/cart'} component={withAuth(Cart)} />
          <Route path={'/auth/callback'} component={Callback} />
          <Route path={'/auth/silent-renew'} component={SilentCallback} />
          <Route path={'/401'} component={NotAuth} />
          <Route component={NotFound} />
        </Switch>
      </Suspense>
    </>
  )
}

export default withRouter(AppLayout)
