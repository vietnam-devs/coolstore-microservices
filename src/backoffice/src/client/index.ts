import { ApolloClient } from 'apollo-client'
import { InMemoryCache } from 'apollo-cache-inmemory'
import { onError } from 'apollo-link-error'
import { ApolloLink } from 'apollo-link'
import { TankaLink, TankaClient } from '@tanka/tanka-graphql-server-link'

const serverClient = new TankaClient(`${process.env.REACT_APP_GRAPHQL_ENDPOINT}/graphql`)
const serverLink = new TankaLink(serverClient)

const client = new ApolloClient({
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
  cache: new InMemoryCache()
})

export default client
