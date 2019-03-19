import { DataProxy } from 'apollo-cache'

export interface MyContext {
  cache: DataProxy
  getCacheKey: (options: { __typename: string; id: string }) => string
}
