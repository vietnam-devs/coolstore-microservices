import gql from 'graphql-tag'
import { MutationResolvers, QueryResolvers } from './generated/graphql'

export const resolvers = {
  // TODO: add more business here
} as {
  Query: QueryResolvers
  Mutation: MutationResolvers
}
