import * as EventEmitter from 'events'
import * as mongoose from 'mongoose'

import { default as Logger } from './logger'
import { ProductModel } from '../models/product'
import { ProductService } from './product'
const ProductData = require('./products.json')

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
  Logger.info(`Connected to ${mongoUri}.`)

  // seed data for this service
  const products = (await ProductService.findProducts()) || []
  if (products.length == 0) {
    Logger.info(
      `Seed data to database #${JSON.stringify(ProductData)} in current database count is #${products.length}`
    )
    ProductData.map(async (model: ProductModel) => {
      Logger.log(model)
      await ProductService.createProduct(model)
    })
  }

  eventEmitter.emit('ready')
})

export { eventEmitter }
