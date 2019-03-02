import * as uuid from 'uuid'
import * as protoLoader from '@grpc/proto-loader'
import * as grpc from 'grpc'

import { default as logger, SimpleLogger as xLogger } from './logger'
import { default as productSchema, ProductModel } from '../models/product'
import { ProductService } from './product-service'
const products = require('./products.json')

const getProto = (protofile: any): any => {
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

const ProductProtoServices = {
  getProducts: async (call: any, callback: any) => {
    logger.info(call.request)
    if (call.request.high_price <= 0) {
      call.request.high_price = Number.MAX_VALUE
    }
    const products = await ProductService.findProducts({ price: { $lt: call.request.high_price } })
    const results = products.map((x: any) => {
      return {
        id: x._id,
        name: x.name,
        desc: x.desc,
        price: x.price,
        image_url: x.imageUrl
      }
    })
    logger.info(results)
    callback(null, { products: results })
  },
  getProductById: async (call: any, callback: any) => {
    logger.info(call.request)
    const product: any = await ProductService.findProduct(call.request.product_id)
    logger.info(product)
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
    logger.info(call.request)
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

const HealthProtoServices = {
  check: async (call: any, callback: any) => {
    callback(null, { status: 'SERVING' })
  }
}

export default async () => {
  const server = new grpc.Server()
  const proto: any = getProto('../proto/catalog.proto').coolstore
  const healthProto: any = getProto('../proto/health.proto').grpc.health.v1

  logger.info(ProductProtoServices)
  await server.addService(proto.CatalogService.service, {
    ...ProductProtoServices
  })
  await server.addService(healthProto.Health.service, {
    ...HealthProtoServices
  })

  server.bind(`${process.env.HOST || '0.0.0.0'}:${process.env.PORT || 5002}`, grpc.ServerCredentials.createInsecure())
  server.start()
  xLogger.info(`gRPC server running on ${process.env.HOST || '0.0.0.0'}:${process.env.PORT || 5002}.`)
  xLogger.info(`Press CTRL-C to stop.`)
}
