require('dotenv').config()

const EventEmitter = require('events')
const pino = require('pino')

const protoLoader = require('@grpc/proto-loader')
const grpc = require('grpc')

const mongoose = require('mongoose')
const productService = require('./services/product')

const logger = pino({
  name: 'catalog-service',
  messageKey: 'message',
  changeLevelName: 'severity',
  useLevelLabels: true
})

const getProto = protofile => {
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

const getProducts = async (call, callback) => {
  logger.info('called endpoint', call.request)

  const results = await productService.allProducts({ high_price: call.request.high_price })
  logger.info(results)
  return callback(null, { products: results })
}

getProductById = async (call, callback) => {
  logger.info('called endpoint', call.request)

  const product = await productService.getProduct(call.request.product_id)
  console.log(product)
  callback(null, { product: product })
}

createProduct = async (call, callback) => {
  let product = new productService({
    ...call.request
  })
  await product.addProduct()
  callback(null, {})
}

const eventEmitter = new EventEmitter()
const mongoUri = `${process.env.MONGO_DB_URL || 'mongodb://localhost/catalog'}`

if (mongoose.connection.readyState == 0) {
  mongoose.set('debug', true)
  console.info(`Trying to connect to ${mongoUri}.`)
  mongoose.connect(mongoUri, {
    useNewUrlParser: true,
    keepAlive: 120
  })
}

mongoose.connection.once('open', function() {
  logger.info(`Connected to ${mongoUri}.`)
  eventEmitter.emit('ready')
})

async function main() {
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
