import { default as logger, SimpleLogger as xLogger } from './logger'
import { ProductModel } from '../models/product'
import { ProductService } from './product-service'
const mongoose = require('mongoose')
const ProductData = require('./products.json')

  // @ts-ignore: ignore to check global variables
;(mongoose as any).Promise = global.Promise
const initDb = async () => {
  mongoose.set('debug', true)
  const mongoUri = `${process.env.MONGO_DB_URI || 'mongodb://localhost/catalog'}`
  let db = await mongoose.connect(mongoUri, {
    useNewUrlParser: true,
    keepAlive: true,
    autoReconnect: true,
    reconnectTries: Number.MAX_VALUE,
    reconnectInterval: 500
  })
  xLogger.info(`Connected to ${mongoUri}.`)

  db.connection.once('open', async () => {
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

  db.connection.on('error', xLogger.error.bind(console, 'MongoDB connection error'))
}

export { initDb }
