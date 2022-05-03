import React, { useEffect } from 'react'
import { RouteChildrenProps } from 'react-router'

import { AuthService, LoggerService } from 'services'

export default (props: React.Component & RouteChildrenProps) => {
  const signinRedirectCallback = async () => {
    try {
      const user = await AuthService.UserManager.signinRedirectCallback()
      LoggerService.info('Successfull token callback.')
      props.history.push(user.state.url)
    } catch (error) {
      LoggerService.error(`There was an error while handling the token callback: ${error}.`)
      props.history.push('/401')
    }
  }

  useEffect(() => {
    signinRedirectCallback()
    // eslint-disable-next-line
  }, [])

  return <div>Authentication callback ...</div>
}
