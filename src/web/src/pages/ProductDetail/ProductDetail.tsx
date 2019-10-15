import React, { useEffect, useCallback } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { Header, Footer } from 'components/App'
import { ProductItemDetail } from 'components/Product'

import { useStore, AppActions } from 'stores/store'
import { getProduct } from 'services/ProductService'

type TParams = {
  id: string
}

interface IProps extends RouteComponentProps<TParams> {}

const ProductDetail: React.FC<IProps> = ({ match }: RouteComponentProps<TParams>) => {
  const { state, dispatch } = useStore()

  const fetchData = useCallback(
    async (id: string) => {
      const product = await getProduct(id)
      dispatch(AppActions.loadProduct(product))
    },
    [dispatch]
  )

  useEffect(() => {
    fetchData(match.params.id)
  }, [state.isProductLoaded, fetchData, match.params.id])

  return (
    <>
      <Header></Header>
      <ProductItemDetail data={state.productDetail}></ProductItemDetail>
      <Footer></Footer>
    </>
  )
}

export default ProductDetail
