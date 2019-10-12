import React from 'react'

import { Header, Footer } from 'components/App'
import { ProductItemDetail } from 'components/Product'

const ProductDetail: React.FC = () => {
  return (
    <>
      <Header></Header>
      <ProductItemDetail></ProductItemDetail>
      <Footer></Footer>
    </>
  )
}

export default ProductDetail
