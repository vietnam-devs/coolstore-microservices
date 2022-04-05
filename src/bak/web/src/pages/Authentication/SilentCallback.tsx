import React, { useEffect } from 'react'
import { RouteChildrenProps } from 'react-router'

import { AuthService, LoggerService } from 'services'

export default (props: React.Component & RouteChildrenProps) => {
  useEffect(() => {
    let signinRedirectCallback = async () => {
      try {
        await AuthService.UserManager.signinSilentCallback()

        LoggerService.info('Successfull token silent callback.')
      } catch (error) {
        LoggerService.error(`There was an error while handling the token callback: ${error}.`)
      }
    }

    signinRedirectCallback()
    // eslint-disable-next-line
  }, [])

  return <div>Authentication callback ...</div>
}
