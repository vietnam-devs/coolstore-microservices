declare var require: any
declare var process: any

let mongoose = require('mongoose')
import { default as Product, ProductCreateRequest } from '../models/product'
let productsData = require('./products.json')

  // @ts-ignore: ignore to check global variables
  ; (mongoose as any).Promise = global.Promise

let isProduction = process.env.NODE_ENV === 'production'
console.info(`Production environment is ${isProduction}`)

let mongoDbUri = process.env.MONGO_DB_URL || 'mongodb://localhost:27017/catalog'

if (mongoose.connection.readyState == 0) {
  mongoose.set('debug', true)
  console.info(`Trying to connect to ${mongoDbUri}.`)
  mongoose.connect(
    mongoDbUri,
    {
      useNewUrlParser: true,
      keepAlive: 120
    }
  )
  var db = mongoose.connection;
  db.once('open', function callback() {
    console.info("First time connect to database")
    Product.find({}, function (err, products) {
      products = products || []
      if (products.length == 0) {
        console.info(`Seed data to database #${JSON.stringify(productsData)} in current database count is #${products.length}`)
        productsData.map(product => {
          Product.create(product, function (err) {
            if (err) console.info(err)
          })
        })
      }
    })
  });
}

export { mongoose, mongoDbUri }
