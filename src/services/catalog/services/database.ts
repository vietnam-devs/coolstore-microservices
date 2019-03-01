import { default as logger, SimpleLogger as xLogger } from './logger'
const mongoose = require('mongoose')
;(mongoose as any).Promise = global.Promise

mongoose.set('debug', true)
const mongoUri = `${process.env.MONGO_DB_URI || 'mongodb://localhost/catalog'}`
mongoose.connect(
  mongoUri,
  {
    useNewUrlParser: true,
    keepAlive: true,
    autoReconnect: true,
    reconnectTries: Number.MAX_VALUE,
    reconnectInterval: 500,
    family: 4
  },
  error => {
    if (error) {
      xLogger.info(`Connect to to ${mongoUri} error with: ${error}`)
    } else {
      xLogger.info(`Connected to ${mongoUri}.`)
    }
  }
)

export default mongoose
