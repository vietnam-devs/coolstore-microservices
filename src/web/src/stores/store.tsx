// ref https://stackblitz.com/edit/react-ts-tg3gfu
import React, { createContext, useContext, useReducer, useEffect } from 'react'
import { User } from 'oidc-client'

import { LoggerService, AuthService } from 'services'

export const LOAD_USER_LOGIN = 'LOAD_USER_LOGIN'
export const UNLOAD_USER_LOGIN = 'UNLOAD_USER_LOGIN'

export interface ActionsWithoutPayload<TypeAction> {
  type: TypeAction
}

export function createAction<TypeAction>(actionType: TypeAction): () => ActionsWithoutPayload<TypeAction> {
  return (): ActionsWithoutPayload<TypeAction> => {
    return {
      type: actionType
    }
  }
}

export function createActionPayload<TypeAction, TypePayload>(
  actionType: TypeAction
): (payload: TypePayload) => ActionsWithPayload<TypeAction, TypePayload> {
  return (p: TypePayload): ActionsWithPayload<TypeAction, TypePayload> => {
    return {
      payload: p,
      type: actionType
    }
  }
}

export interface ActionsWithPayload<TypeAction, TypePayload> {
  type: TypeAction
  payload: TypePayload
}

interface ActionCreatorsMapObject {
  [key: string]: (...args: any[]) => ActionsWithPayload<any, any> | ActionsWithoutPayload<any>
}

export type ActionsUnion<A extends ActionCreatorsMapObject> = ReturnType<A[keyof A]>

interface IUser {
  username: string
  email: string
}

interface IAction {
  type: string
}

interface IAppState {
  user: IUser | null
  authenticated: boolean
}

interface IAppContextProps {
  state: IAppState
  dispatch: ({ type }: IAction) => void
}

const initialState: IAppState = {
  user: null,
  authenticated: false
}

const Actions = {
  loadUserLogin: createActionPayload<typeof LOAD_USER_LOGIN, IUser>(LOAD_USER_LOGIN),
  unloadUserLogin: createAction<typeof UNLOAD_USER_LOGIN>(UNLOAD_USER_LOGIN)
}

const reducers = (state: IAppState, action: ActionsUnion<typeof Actions>) => {
  switch (action.type) {
    case LOAD_USER_LOGIN:
      return {
        ...state,
        user: action.payload,
        authenticated: true
      }
    case UNLOAD_USER_LOGIN:
      return {
        ...state,
        user: null,
        authenticated: false
      }
    default:
      return initialState
  }
}

const onUserLoaded = (dispatch: React.Dispatch<any>) => (user: User) => {
  LoggerService.info('User Loaded.')
  dispatch(
    Actions.loadUserLogin({
      username: user.profile.preferred_username,
      email: user.profile.email
    })
  )
}

const onUserUnloaded = (dispatch: React.Dispatch<any>) => () => {
  LoggerService.info('User unloaded.')
  dispatch(Actions.unloadUserLogin())
}

const onAccessTokenExpired = (dispatch: React.Dispatch<any>) => async () => {
  LoggerService.info('Token expired.')
  dispatch(Actions.unloadUserLogin())
  await AuthService.UserManager.signinSilent()
}

const addOidcEvents = (dispatch: React.Dispatch<any>) => {
  const oidcEvents = AuthService.UserManager.events

  oidcEvents.addUserLoaded(onUserLoaded(dispatch))
  oidcEvents.addUserUnloaded(onUserUnloaded(dispatch))
  oidcEvents.addAccessTokenExpired(onAccessTokenExpired(dispatch))
}

const removeOidcEvents = (dispatch: React.Dispatch<any>) => {
  const oidcEvents = AuthService.UserManager.events

  oidcEvents.removeUserLoaded(onUserLoaded(dispatch))
  oidcEvents.removeUserUnloaded(onUserUnloaded(dispatch))
  oidcEvents.removeAccessTokenExpired(onAccessTokenExpired(dispatch))
}

const AppContext = createContext({} as IAppContextProps)

const store = {}

export const AppProvider = (props: any) => {
  const [state, dispatch] = useReducer(reducers, initialState)
  const value = { state, dispatch } as IAppContextProps

  useEffect(() => {
    addOidcEvents(dispatch)

    AuthService.UserManager.getUser().then(oidcUser => {
      if (oidcUser && !oidcUser!.expired) {
        dispatch(
          Actions.loadUserLogin({
            username: oidcUser.profile.preferred_username,
            email: oidcUser.profile.email
          })
        )
      }
    })

    return () => removeOidcEvents(dispatch)
  }, [])

  return <AppContext.Provider value={value}>{props.children}</AppContext.Provider>
}

export const useStore = () => useContext(AppContext)
