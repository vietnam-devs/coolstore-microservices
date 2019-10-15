import React from 'react'
import { RouteChildrenProps } from 'react-router'

import { Header, Footer } from 'components/App'

const withLayout = <P extends object>(WrappedComponent: React.ComponentType<P>) => {
  return class extends React.Component<P & RouteChildrenProps> {
    render() {
      return (
        <>
          <Header></Header>
          <WrappedComponent {...this.props} />
          <Footer></Footer>
        </>
      )
    }
  }
}

export default withLayout
