import React, { useEffect, useCallback } from 'react'
import { RouteChildrenProps } from 'react-router'
import { AuthService } from 'services'

const withAuth = <P extends object>(WrappedComponent: React.ComponentType<P>) => {
  return function({ ...props }: P & RouteChildrenProps) {
    const location = { ...(props as RouteChildrenProps) }
    const authUser = useCallback(async () => {
      await AuthService.authenticateUser(location)
    }, [location])

    useEffect(() => {
      authUser()
    }, [authUser])

    return <>{<WrappedComponent {...props} />}</>
  }
}

export default withAuth
