import * as uuid from 'uuid'
import { default as Logger } from './logger'
import { default as productModel, ProductModel } from '../models/product'

export const ProductProtoServices = {
  getProducts: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    if (call.request.high_price <= 0) {
      call.request.high_price = Number.MAX_VALUE
    }
    const products = await productModel.find({ price: { $lt: call.request.high_price } }).exec()
    const results = products.map((x: any) => {
      return {
        id: x._id,
        name: x.name,
        desc: x.desc,
        price: x.price,
        image_url: x.imageUrl
      }
    })
    Logger.info(results)
    callback(null, { products: results })
  },
  getProductById: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const product: any = await ProductService.findProduct(call.request.product_id)
    callback(null, {
      product: {
        id: product._id,
        name: product.name,
        desc: product.desc,
        price: product.price,
        image_url: product.imageUrl
      }
    })
  },
  createProduct: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const model: ProductModel = {
      id: uuid.v1(),
      name: call.request.name,
      desc: call.request.desc,
      price: call.request.price,
      imageUrl: call.request.image_url
    }
    const product: any = await ProductService.createProduct(model)
    callback(null, {
      product: {
        id: product._id,
        name: product.name,
        desc: product.desc,
        price: product.price,
        image_url: product.imageUrl
      }
    })
  }
}

export class ProductService {
  static async findProduct(productId: string) {
    return await productModel.findOne({ _id: productId }).exec()
  }
  static async findProducts() {
    return await productModel.find({}).exec()
  }
  static async createProduct(model: ProductModel) {
    const id = uuid.v1()
    Logger.info({ ...model, _id: id })
    return await new productModel({ ...model, _id: id }).save()
  }
}
