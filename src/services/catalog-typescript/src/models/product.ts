import * as mongoose from 'mongoose'
const uuid = require('uuid/v1')

const Schema = mongoose.Schema

export interface Product {
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

let productSchema = new Schema({
  _id: {
    type: String,
    required: 'Enter a id'
  },
  name: {
    type: String,
    required: 'Product name cannot be blank'
  },
  desc: {
    type: String
  },
  price: {
    type: Number,
    required: 'Price cannot be blank'
  },
  imageUrl: {
    type: String,
    required: 'Image cannot be blank'
  }
})



// Duplicate the ID field.
productSchema.virtual('id').get(function () {
  return this._id;
});

// Ensure virtual fields are serialised.
productSchema.set('toJSON', {
  virtuals: true
});

productSchema.path('name').required(true, 'Product name cannot be blank')
productSchema.path('price').required(true, 'price cannot be blank')
productSchema.path('price').validate(function (price) {
  // https://gist.github.com/rutcreate/03ff3f9bd5f414465322
  return Number(price).toString() === price.toString()
}, 'Price must be a float number.')
productSchema.path('imageUrl').required(true, 'Image cannot be blank')

productSchema.methods = {
  createProduct: function (product) {
    console.log(product)
    this._id = uuid()
    this.name = product.name
    this.desc = product.desc
    this.price = product.price
    this.imageUrl = product.imageUrl
    return this
  }
}

productSchema.statics = {
  findProduct: function (_id) {
    return this.find({ _id }).exec()
  },
  findProducts: function (options) {
    return this.find({}).exec()
  }
}

export default productSchema