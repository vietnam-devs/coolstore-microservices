import { mongoose } from '../config/database'
import { Document, Model, Schema } from 'mongoose'
import uuid from 'uuid/v1'

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

export interface ProductModel extends Model<ProductDoc> {
  createProduct(product: ProductCreateRequest): Promise<{ product: ProductDoc }>
}

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
  // https://gist.github.com/rutcreate/03ff3f9bd5f414465322
  return Number(price).toString() === price.toString()
}, `Price must be a float number.`)
productSchema.path('imageUrl').required(true, `Image can't be blank.`)

productSchema.static('createProduct', (product: ProductCreateRequest) => {
  console.log(product)
  return Product.create({
    _id: uuid(),
    ...product
  })
    .then(product => {
      return product
    })
    .catch(ex => {})
})

export const Product = mongoose.model<ProductDoc>(
  'Product',
  productSchema
) as ProductModel
