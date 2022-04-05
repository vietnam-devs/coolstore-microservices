import React, { useEffect } from 'react'
import { RouteChildrenProps } from 'react-router'
import { RouteComponentProps } from 'react-router-dom'

import { Header, Footer, Notification } from 'components/App'
import { AppActions, useStore } from 'stores/store'

const withLayout = <P extends RouteComponentProps>(WrappedComponent: React.ComponentType<P>) => {
  return function({ ...props }: P & RouteChildrenProps) {
    const { dispatch } = useStore()

    useEffect(() => {
      const timer = setTimeout(() => {
        dispatch(AppActions.hideNotification())
      }, 3000)
      return () => clearTimeout(timer)
    })

    return (
      <>
        <Header {...props}></Header>
        <Notification></Notification>
        <WrappedComponent {...props} />
        <Footer></Footer>
      </>
    )
  }
}

export default withLayout
