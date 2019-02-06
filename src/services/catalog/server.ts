require('dotenv').config()

import * as EventEmitter from 'events'
import * as Pino from 'pino'
import * as protoLoader from '@grpc/proto-loader'
import * as grpc from 'grpc'
import * as mongoose from 'mongoose'

import { default as ProductService, AddProductRequest } from './services/product'
const ProductData = require('./products.json')

const logger = Pino({
  name: 'catalog-service',
  messageKey: 'message',
  changeLevelName: 'severity',
  useLevelLabels: true
})

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

const getProducts = async (call: any, callback: any) => {
  logger.info('request', call.request)
  const results = await ProductService.allProducts({ high_price: call.request.high_price })
  logger.info(results)
  return callback(null, { products: results })
}

const getProductById = async (call: any, callback: any) => {
  logger.info('request', call.request)
  const product = await ProductService.getProduct(call.request.product_id)
  callback(null, { product: product })
}

const createProduct = async (call: any, callback: any) => {
  logger.info('request', call.request)
  await ProductService.createProduct({ ...call.request })
  callback(null, {})
}

const eventEmitter = new EventEmitter()
const mongoUri = `${process.env.MONGO_DB_URL || 'mongodb://localhost/catalog'}`
if (mongoose.connection.readyState == 0) {
  mongoose.set('debug', true)
  console.info(`Trying to connect to ${mongoUri}.`)
  mongoose.connect(mongoUri, {
    useNewUrlParser: true,
    keepAlive: true
  })
}

mongoose.connection.once('open', async () => {
  logger.info(`Connected to ${mongoUri}.`)
  const products = (await ProductService.findProducts()) || []
  if (products.length == 0) {
    logger.info(
      `Seed data to database #${JSON.stringify(ProductData)} in current database count is #${products.length}`
    )
    ProductData.map(async (p: AddProductRequest) => {
      await ProductService.createProduct(p)
    })
  }

  eventEmitter.emit('ready')
})

const main = async () => {
  const server = new grpc.Server()
  const proto = getProto('./proto/catalog.proto')

  await server.addService(proto.coolstore.CatalogService.service, {
    getProducts,
    getProductById,
    createProduct
  })

  server.bind(`0.0.0.0:${process.env.PORT || 5002}`, grpc.ServerCredentials.createInsecure())
  server.start()
  logger.info(`gRPC server running on port ${process.env.PORT}.\n Press CTRL-C to stop.\n`)
}

eventEmitter.on('ready', async () => {
  await main().catch(console.error)
})
