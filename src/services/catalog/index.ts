require('dotenv').config()
const mongoose = require('mongoose')
import { default as logger, SimpleLogger as xLogger } from './services/logger'
//import { initDb } from './services/database'
import initServer from './services/grpc-server'
import { ProductModel } from './models/product'
import { ProductService } from './services/product-service'
const ProductData = require('./services/products.json')

/*initDb().then(
  async () => {
    await initServer()
  },
  error => xLogger.error(error.stack)
)*/
/*;(mongoose as any).Promise = global.Promise

mongoose.set('debug', true)
const mongoUri = `${process.env.MONGO_DB_URI || 'mongodb://localhost/catalog'}`
mongoose.connect(mongoUri, {
  useNewUrlParser: true,
  keepAlive: true,
  autoReconnect: true,
  reconnectTries: Number.MAX_VALUE,
  reconnectInterval: 500
})
xLogger.info(`Connected to ${mongoUri}.`)

mongoose.connection.once('open', async () => {
  const products = (await ProductService.findProducts()) || []
  if (products.length <= 0) {
    await ProductData.map(async (model: ProductModel) => {
      logger.info(model)
      await ProductService.createProduct(model)
    })
    xLogger.info(
      `Seed data to database #${JSON.stringify(ProductData)} in current database count is #${products.length}`
    )
  }
})

mongoose.connection.on('error', xLogger.error.bind(console, 'MongoDB connection error'))*/

initServer()
