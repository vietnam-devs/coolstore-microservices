// ref https://stackblitz.com/edit/react-ts-tg3gfu
import React, { createContext, useContext, useReducer, Reducer } from 'react'

interface IAppContextProps<T> {
  state: T
  dispatch: ({ type }: { type: string }) => void
}

interface IAppState {}

const initialState: IAppState = {}

type Actions = ILoadProducts

const reducers = (state: IAppState, action: Actions) => {
  switch (action.type) {
    case 'LOAD_PRODUCTS':
      return { ...state }
  }
}

const AppContext = createContext(initialState as IAppContextProps<IAppState>)

const store = {}

interface ILoadProducts {
  type: 'LOAD_PRODUCTS'
  value: boolean
}

export const AppProvider = (props: any) => {
  const [state, dispatch] = useReducer<Reducer<IAppState, any>>(reducers, initialState)
  const value = { state, dispatch }
  return <AppContext.Provider value={value}>{props.children}</AppContext.Provider>
}

export const useStore = () => useContext(AppContext)
