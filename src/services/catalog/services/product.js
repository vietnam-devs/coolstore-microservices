let productModel = require('../models/product')

let Product = class {
  constructor(payload) {
    this.payload = payload
  }

  allProducts(cb) {
    if (this.payload.highPrice <= 0) {
      this.payload.highPrice = Number.MAX_VALUE
    }
    productModel.find({ price: { $lt: this.payload.price } }, cb)
  }

  getProduct(cb) {
    productModel.findOne({ _id: this.payload.product_id }, cb)
  }

  addProduct(cb) {
    new productModel(this.payload).save(cb)
  }
}

module.exports = Product
