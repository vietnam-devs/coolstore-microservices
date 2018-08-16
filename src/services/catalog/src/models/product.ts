import { mongoose } from '../config/database'
import { Document, Model, Schema } from 'mongoose'
import v1 = require('uuid/v1')

export interface ProductDoc extends Document {
  id: string
  name: string
  desc: string
  price: number
  imageUrl: string
}

export interface ProductCreateRequest {
  name: string
  desc: string
  price: number
  imageUrl: string
}

/*export interface ProductModel extends Model<ProductDoc> {
  createProduct(product: ProductCreateRequest): Promise<{ product: ProductDoc }>
}*/

let productSchema = new Schema({
  _id: {
    type: String,
    required: `Enter an guid id.`
  },
  name: {
    type: String,
    required: `Product name can't be blank.`
  },
  desc: {
    type: String
  },
  price: {
    type: Number,
    required: `Price can't be blank.`
  },
  imageUrl: {
    type: String,
    required: `Image can't be blank.`
  }
})

// Duplicate the ID field.
productSchema.virtual('id').get(function() {
  return this._id
})

// Ensure virtual fields are serialised.
productSchema.set('toJSON', {
  virtuals: true
})

productSchema.path('name').required(true, `Product name can't be blank.`)
productSchema.path('price').required(true, `Price can't be blank.`)
productSchema.path('price').validate(function(price) {
  return Number(price).toString() === price.toString()
}, `Price must be a float number.`)
productSchema.path('imageUrl').required(true, `Image can't be blank.`)

const Product = mongoose.model('Product', productSchema)
export default Product
