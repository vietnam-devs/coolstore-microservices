import React from 'react'
import PropTypes from 'prop-types'

import gql from 'graphql-tag'
import { useQuery, useApolloClient } from 'react-apollo-hooks'

import { Theme, createStyles, withStyles, WithStyles } from '@material-ui/core/styles'
import Typography from '@material-ui/core/Typography'
import Grid from '@material-ui/core/Grid'
import Button from '@material-ui/core/Button'

import ProductPriceSearch from '../components/ProductPriceSearch'
import ProductItem from '../components/ProductItem'
import withRoot from '../withRoot'

const GET_PRODUCTS = gql`
  query GetProducts($message: GetProductsInput!) {
    products(input: $message) {
      id
      name
      price
      imageUrl
      desc
    }
  }
`

const GET_PRODUCT_SEARCH = gql`
  query ProductSearch {
    productSearch @client {
      price
      page
    }
  }
`

const styles = (theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1
    },
    button: {
      margin: theme.spacing.unit
    }
  })

interface ProductListProps extends WithStyles<typeof styles> {}

const ProductList: React.FC<ProductListProps> = ({ classes }: ProductListProps) => {
  const client = useApolloClient()

  const productSearchQuery = useQuery(GET_PRODUCT_SEARCH)

  const { page, price } = productSearchQuery.data.productSearch

  const { data, error, loading } = useQuery(GET_PRODUCTS, {
    variables: {
      message: {
        currentPage: page,
        highPrice: price
      }
    }
  })

  const setPrice = (price: number): void => {
    client.writeData({
      data: {
        productSearch: {
          __typename: 'ProductSearch',
          price: price,
          page: page
        }
      }
    })
  }

  const searchWithTitle = (
    <>
      <Typography variant="h4" color="inherit">
        Product List
      </Typography>
      <Button variant="contained" color="primary" className={classes.button}>
        Add Product
      </Button>
      <hr />
      <ProductPriceSearch
        price={price}
        onPriceChange={e => {
          setPrice(parseInt(e.target.value || '0'))
        }}
      />
      <br />
    </>
  )

  if (loading) {
    return (
      <div>
        {searchWithTitle} <div> Loading...</div>
      </div>
    )
  }
  if (error) {
    return <div>Error! {error.message}</div>
  }

  return (
    <div className={classes.root}>
      {searchWithTitle}
      <Grid container spacing={24}>
        {data.products.map((value: any, index: number) => {
          return (
            <Grid item xs={3} key={index}>
              <ProductItem name={value.name} description={value.desc} imageUrl={value.imageUrl} price={value.price} />
            </Grid>
          )
        })}
      </Grid>
    </div>
  )
}

ProductList.propTypes = {
  classes: PropTypes.object.isRequired
} as any

export default withRoot(withStyles(styles)(ProductList))
