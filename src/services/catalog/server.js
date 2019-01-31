require('dotenv').config()

const grpc = require('grpc')
const pino = require('pino')
const protoLoader = require('@grpc/proto-loader')
global.Mongoose = require('mongoose')

const proto = getProto('./proto/catalog.proto')
const productService = require('./services/product')

const server = new grpc.Server()

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

let getProto = () => {
  const packageDefinition = protoLoader.loadSync(
    `./proto/catalog.proto`,
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

server.addService(proto.coolstore.CatalogService.service, {
  GetProducts(call, callback) {
    let product = new productService({ price: call.request.highPrice })
    product.allProducts(callback)
  },
  GetProductById(call, callback) {
    let product = new productService({ product_id: call.request.product_id })
    product.getProduct(callback)
  },
  CreateProduct(call, callback) {
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
  const server = new grpc.Server()
  server.bind(`0.0.0.0:${process.env.PORT || 5002}`, grpc.ServerCredentials.createInsecure())
  server.start()
  logger.info(`gRPC server running on port ${process.env.PORT}.\n Press CTRL-C to stop.\n`)
}

main()
