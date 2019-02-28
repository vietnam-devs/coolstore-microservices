import * as uuid from 'uuid'
import { default as Logger } from './logger'
import { default as productSchema, ProductModel } from '../models/product'
const ProductData = require('./products.json')

export class ProductService {
  static async findProduct(productId: string) {
    Logger.info("ProductService find product with product id "+ productId)
    return await productSchema.findOne({ _id: productId }).exec()
  }
  static async findProducts(filter?: any) {
    Logger.info("ProductService find all product with filter "+ filter)
    try {
      await productSchema.find(filter).exec()
    } catch (error) {
      Logger.info(error)
    }
    return await productSchema.find(filter).exec()
  }
  static async createProduct(model: ProductModel) {
    const id = uuid.v1()
    Logger.info({ ...model, _id: id })
    return await new productSchema({ ...model, _id: id }).save()
  }

  static async initDb(){
    var products = await productSchema.find().exec()
    Logger.info("Init db with products count in db: " + products.length)
    if(products.length <= 0){
      await ProductData.map(async (model: ProductModel) => {
        await ProductService.createProduct(model)
      })
    }
  }
}
