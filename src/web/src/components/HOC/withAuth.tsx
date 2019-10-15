import React from 'react'
import { RouteChildrenProps } from 'react-router'

import { AuthService } from 'services'

const withAuth = <P extends object>(WrappedComponent: React.ComponentType<P>) => {
  return class extends React.Component<P & RouteChildrenProps> {
    async componentDidMount() {
      let location = { ...(this.props as RouteChildrenProps) }
      AuthService.authenticateUser(location)
    }

    render() {
      return <WrappedComponent {...this.props} />
    }
  }
}

export default withAuth
