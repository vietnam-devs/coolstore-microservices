import { ApolloClient } from 'apollo-client'
import { InMemoryCache } from 'apollo-cache-inmemory'
import { onError } from 'apollo-link-error'
import { ApolloLink } from 'apollo-link'
import { TankaLink, TankaClient } from '@tanka/tanka-graphql-server-link'

import { AuthService } from './services/AuthService'
import { resolvers } from './resolvers'

const client = async () => {
  const cache = new InMemoryCache()
  const authService: AuthService = new AuthService()
  const user = await authService.getUser()
  const options = {
    accessTokenFactory: () => (user != null && user.access_token != null ? user.access_token : '')
  }

  const serverClient = new TankaClient(`${process.env.REACT_APP_GRAPHQL_ENDPOINT}/graphql`, options)
  const serverLink = new TankaLink(serverClient)

  return new ApolloClient({
    connectToDevTools: true,
    link: ApolloLink.from([
      onError(({ graphQLErrors, networkError }) => {
        if (graphQLErrors)
          graphQLErrors.map(({ message, locations, path }) =>
            console.log(`[GraphQL error]: Message: ${message}, Location: ${locations}, Path: ${path}`)
          )
        if (networkError) console.log(`[Network error]: ${networkError}`)
      }),
      serverLink
    ]),
    cache: cache,
    resolvers: resolvers as any
  })
}

export default client
