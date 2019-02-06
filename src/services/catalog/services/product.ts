import * as uuid from 'uuid'
import productModel from '../models/product'

export default class {
  static async allProducts(options) {
    if (options.high_price <= 0) {
      options.high_price = Number.MAX_VALUE
    }
    return await productModel.find({ price: { $lt: options.high_price } }).exec()
  }

  static async getProduct(productId) {
    return await productModel.findOne({ _id: productId }).exec()
  }

  static async findProducts() {
    return await productModel.find({}).exec()
  }

  static async createProduct(payload: AddProductRequest) {
    console.log({ ...payload, _id: uuid.v1() })
    await new productModel({ ...payload, _id: uuid.v1() }).save()
  }
}

export interface AddProductRequest {
  id: string
  name: string
  desc: string
  price: number
  imageUrl: string
}
