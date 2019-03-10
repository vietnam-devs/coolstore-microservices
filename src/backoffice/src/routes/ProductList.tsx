import React from 'react'
import gql from 'graphql-tag'
import { Query } from 'react-apollo'

const GET_PRODUCTS = gql`
  query GetProducts($message: GetProductsInput!) {
    products(input: $message) {
      id
      name
    }
  }
`

const ProductList = () => {
  return (
    <Query query={GET_PRODUCTS} variables={{ message: { currentPage: 1, highPrice: 999 } }}>
      {({ loading, error, data }) => {
        if (loading) return 'Loading..'
        if (error) return `Error: ${error}`
        console.log(data)
        return (
          <div>
            <h3>Product List</h3>
            <ul>
              {data.products.map((value: any, index: any) => {
                return <li key={index}>{value.name}</li>
              })}
            </ul>
          </div>
        )
      }}
    </Query>
  )
}

export default ProductList
