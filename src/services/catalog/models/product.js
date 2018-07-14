const uuid = require('uuid/v1')
const mongoose = require('mongoose')

const Schema = mongoose.Schema

var ProductSchema = new Schema({
  _id: { type: String, default: uuid },
  name: { type: String },
  desc: { type: String },
  price: { type: Number }
})

ProductSchema.path('name').required(true, 'Product name cannot be blank')
ProductSchema.path('price').required(true, 'price cannot be blank')
ProductSchema.path('price').validate(function(price) {
  // https://gist.github.com/rutcreate/03ff3f9bd5f414465322
  return Number(price).toString() === price.toString() && price % 1 !== 0
}, 'Price must be a float number.')

ProductSchema.methods = {
  createProduct: function(product) {
    this._id = uuid()
    this.name = product.name
    this.desc = product.desc
    this.price = product.price
    return this.save()
  }
}

ProductSchema.statics = {
  findProduct: function(_id) {
    return this.find({ _id }).exec()
  },
  findProducts: function(options) {
    return this.find({}).exec()
  }
}

mongoose.model('Product', ProductSchema)
