import React, { useState, useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { Header, Footer } from 'components/App'
import { ProductItemDetail } from 'components/Product'

import { useStore, AppActions } from 'stores/store'
import { IProduct } from 'stores/types'
import { getProduct } from 'services/ProductService'

type TParams = {
  id: string
}

interface IProps extends RouteComponentProps<TParams> {}

const ProductDetail: React.FC<IProps> = ({ match }: RouteComponentProps<TParams>) => {
  const { state, dispatch } = useStore()
  const [product, setProduct] = useState<IProduct>(null)

  async function fetchData() {
    const product = await getProduct(match.params.id)
    setProduct(product)
    dispatch(AppActions.loadProduct(product))
  }

  useEffect(() => {
    fetchData()
  }, [state.isProductLoaded])

  return (
    <>
      <Header></Header>
      <ProductItemDetail data={product}></ProductItemDetail>
      <Footer></Footer>
    </>
  )
}

export default ProductDetail
