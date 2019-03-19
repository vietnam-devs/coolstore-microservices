import React, { useState, useEffect } from 'react'
import gql from 'graphql-tag'
import { useQuery } from 'react-apollo-hooks'

const GET_PRODUCTS = gql`
  query GetProducts($message: GetProductsInput!) {
    products(input: $message) {
      id
      name
    }
  }
`

interface Props {}

const ProductList: React.FC<Props> = () => {
  const [price, setPrice] = useState(999)
  const [page, _] = useState(1)

  const { data, error, loading } = useQuery(GET_PRODUCTS, {
    variables: {
      message: {
        currentPage: page,
        highPrice: price
      }
    }
  })

  if (loading) {
    return <div>Loading...</div>
  }
  if (error) {
    return <div>Error! {error.message}</div>
  }

  const SearchPrice = () => {
    return (
      <div>
        Price:
        <input
          type="text"
          value={price}
          onChange={e => {
            setPrice(parseInt(e.target.value))
            e.target.focus()
          }}
        />
      </div>
    )
  }

  return (
    <div>
      <h3>Product List</h3>
      <SearchPrice />

      <ul>
        {data.products.map((value: any, index: any) => {
          return <li key={index}>{value.name}</li>
        })}
      </ul>
    </div>
  )
}

export default ProductList
