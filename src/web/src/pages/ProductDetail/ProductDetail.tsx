import React, { useEffect } from 'react'
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

  async function fetchData() {
    const product = await getProduct(match.params.id)
    dispatch(AppActions.loadProduct(product))
  }

  useEffect(() => {
    fetchData()
  }, [state.isProductLoaded])

  return (
    <>
      <Header></Header>
      <ProductItemDetail data={state.productDetail}></ProductItemDetail>
      <Footer></Footer>
    </>
  )
}

export default ProductDetail
