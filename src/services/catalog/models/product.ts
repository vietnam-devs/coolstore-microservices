const mongoose = require('mongoose')
const { Schema } = mongoose
import mongoosedb from "../services/database";


export interface ProductModel {
  id: string
  name: string
  desc: string
  price: number
  imageUrl: string
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
productSchema.virtual('id').get(() => {
  return this._id
})

// Ensure virtual fields are serialised.
productSchema.set('toJSON', {
  virtuals: true
})

productSchema.path('name').required(true, `Product name can't be blank.`)
productSchema.path('price').required(true, `Price can't be blank.`)
/*productSchema.path('price').validate(function(price) {
  return Number(price).toString() === price.toString()
}, `Price must be a float number.`)*/
productSchema.path('imageUrl').required(true, `ImageUri can't be blank.`)
export default mongoosedb.model('Product', productSchema)
