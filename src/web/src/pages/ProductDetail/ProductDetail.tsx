import React, { useState, useEffect } from 'react'
import { RouteComponentProps } from 'react-router-dom'
import axios from 'axios'

import { Header, Footer } from 'components/App'
import { ProductItemDetail } from 'components/Product'
import { useStore, IProduct, AppActions } from 'stores/store'

type TParams = {
  id: string
}

interface IProps extends RouteComponentProps<TParams> {}

const ProductDetail: React.FC<IProps> = ({ match }: RouteComponentProps<TParams>) => {
  const { state, dispatch } = useStore()
  const [product, setProduct] = useState<IProduct>(null)

  const loadProduct = async (id: string) => {
    const result = await axios.get(`/api/products/${id}`, {
      baseURL: `${process.env.REACT_APP_API}`,
      data: {},
      headers: {
        ['Content-Type']: 'application/grpc',
        Authorization: `Bearer ${state.accessToken}`
      }
    })
    setProduct(result.data.product)
    dispatch(AppActions.loadProduct(result.data.product))
  }

  useEffect(() => {
    if (state.accessToken != null && state.productDetail == null) {
      loadProduct(match.params.id)
    } else {
      setProduct(state.productDetail)
    }
  }, [state])

  return (
    <>
      <Header></Header>
      <ProductItemDetail data={product}></ProductItemDetail>
      <Footer></Footer>
    </>
  )
}

export default ProductDetail
