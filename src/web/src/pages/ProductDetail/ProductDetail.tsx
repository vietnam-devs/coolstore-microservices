import React, { useEffect, useCallback } from 'react'
import { RouteComponentProps } from 'react-router-dom'

import { ProductItemDetail } from 'components/Product'
import { withLayout } from 'components/HOC'

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
    <div className="container-fluid">
      <div className="row">
        <div className="offset-xl-2 col-xl-8 col-lg-12 col-md-12 col-sm-12 col-12">
          <div className="row">{state.isProductLoaded && <ProductItemDetail data={state.productDetail} />}</div>
        </div>
      </div>
    </div>
  )
}

export default withLayout(ProductDetail)
