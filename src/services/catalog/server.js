require('dotenv').config()

const protoLoader = require('@grpc/proto-loader')
const grpc = require('grpc')
const pino = require('pino')
global.Mongoose = require('mongoose')

const productService = require('./services/product')

const logger = pino({
  name: 'catalog-service',
  messageKey: 'message',
  changeLevelName: 'severity',
  useLevelLabels: true
})

Mongoose.connect(`${process.env.MONGO_DB_URL || "mongodb://localhost/catalog"}`)
Mongoose.connection.once('open', function() {
  logger.info(`Connected to ${process.env.MONGO_DB_URL}.`)
})

const getProto = (protofile) => {
  const packageDefinition = protoLoader.loadSync(
    protofile,
    {
      keepCase: true,
      longs: String,
      enums: String,
      defaults: true,
      oneofs: true,
      includeDirs: ['node_modules/google-proto-files', 'proto']
    }
  )
  return grpc.loadPackageDefinition(packageDefinition)
}

const server = new grpc.Server()
const proto = getProto('./proto/catalog.proto')

server.addService(proto.coolstore.CatalogService.service, {
  GetProducts: (call, callback) => {
    let product = new productService({ price: call.request.high_price })
    product.allProducts(callback)
  },
  GetProductById: (call, callback) => {
    let product = new productService({ product_id: call.request.product_id })
    product.getProduct(callback)
  },
  CreateProduct: (call, callback) => {
    let product = new productService({
      name: call.request.name,
      desc: call.request.desc,
      price: call.request.price,
      image_url: call.request.image_url
    })
    product.addProduct(callback)
  }
})

function main() {
  server.bind(`0.0.0.0:${process.env.PORT || 5002}`, grpc.ServerCredentials.createInsecure())
  server.start()
  logger.info(`gRPC server running on port ${process.env.PORT}.\n Press CTRL-C to stop.\n`)
}

main()
