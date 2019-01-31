let productModel = require('../models/product')

let Product = class {
  constructor(payload) {
    this.payload = payload
  }

  allProducts(cb) {
    if (this.payload.high_price <= 0) {
      this.payload.high_price = Number.MAX_VALUE
    }
    productModel.find({ price: { $lt: this.payload.high_price } }, cb)
  }

  getProduct(cb) {
    productModel.findOne({ _id: this.payload.product_id }, cb)
  }

  addProduct(cb) {
    new productModel(this.payload).save(cb)
  }
}

module.exports = Product
