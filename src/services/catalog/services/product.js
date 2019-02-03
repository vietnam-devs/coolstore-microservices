const uuid = require('uuid')
let productModel = require('../models/product')

let Product = class {
  constructor(payload) {
    this.payload = payload
  }

  static async allProducts(options) {
    if (options.high_price <= 0) {
      options.high_price = Number.MAX_VALUE
    }
    return await productModel.find({ price: { $lt: options.high_price } }).exec()
  }

  static async getProduct(productId) {
    return await productModel.findOne({ _id: productId }).exec()
  }

  async addProduct() {
    console.log({ ...this.payload, _id: uuid.v1() })
    await new productModel({ ...this.payload, _id: uuid.v1() }).save()
  }
}

module.exports = Product
