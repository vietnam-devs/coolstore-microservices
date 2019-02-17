import * as uuid from 'uuid'
import { default as Logger } from './logger'
import { default as productSchema, ProductModel } from '../models/product'

export class ProductService {
  static async findProduct(productId: string) {
    return await productSchema.findOne({ _id: productId }).exec()
  }
  static async findProducts() {
    return await productSchema.find({}).exec()
  }
  static async createProduct(model: ProductModel) {
    const id = uuid.v1()
    Logger.info({ ...model, _id: id })
    return await new productSchema({ ...model, _id: id }).save()
  }
}
