declare var require: any
declare var process: any

let mongoose = require('mongoose')

// @ts-ignore: ignore to check global variables
;(mongoose as any).Promise = global.Promise

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
}

export { mongoose, mongoDbUri }
