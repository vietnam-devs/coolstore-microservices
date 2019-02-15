import * as uuid from 'uuid'
import * as protoLoader from '@grpc/proto-loader'
import * as grpc from 'grpc'
import { default as Logger } from './logger'
import { default as productSchema, ProductModel } from '../models/product'
import { ProductService } from './product'

const ProductProtoServices = {
  getProducts: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    if (call.request.high_price <= 0) {
      call.request.high_price = Number.MAX_VALUE
    }
    const products = await productSchema.find({ price: { $lt: call.request.high_price } }).exec()
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

const getProto = (protofile: any) => {
  const packageDefinition = protoLoader.loadSync(protofile, {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
    includeDirs: ['node_modules/google-proto-files', 'proto']
  })
  return grpc.loadPackageDefinition(packageDefinition)
}

export default async () => {
  const server = new grpc.Server()
  const proto: any = getProto('../proto/catalog.proto')

  Logger.info(ProductProtoServices)
  await server.addService(proto.coolstore.CatalogService.service, {
    ...ProductProtoServices
  })

  server.bind(`0.0.0.0:${process.env.PORT || 5002}`, grpc.ServerCredentials.createInsecure())
  server.start()
  Logger.info(`gRPC server running on port ${process.env.PORT}.\n Press CTRL-C to stop.\n`)
}
