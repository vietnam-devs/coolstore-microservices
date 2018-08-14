import * as mongoose from 'mongoose'
;(mongoose as any).Promise = global.Promise

let isProduction = process.env.NODE_ENV === 'production'
console.info(`Production environment is ${isProduction}`)

let mongoDbUri = 'mongodb://localhost:27017/catalog'
if (isProduction) {
  mongoDbUri = process.env.MONGO_DB_URL
}

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
