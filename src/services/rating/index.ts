require('dotenv').config()
const mongoose = require('mongoose')
import { SimpleLogger as xLogger } from './services/logger'
import { seedData } from './services/database'
import initServer from './services/grpc-server'

// @ts-ignore: ignore to check global variables
;(mongoose as any).Promise = global.Promise

const run = async () => {
  mongoose.set('debug', true)
  const mongoUri = `${process.env.MONGO_DB_URI || 'mongodb://localhost/rating'}`
  await mongoose.connect(mongoUri, {
    useNewUrlParser: true,
    keepAlive: true,
    autoReconnect: true,
    reconnectTries: 1000000,
    reconnectInterval: 3000,
    family: 4
  })
  xLogger.info(`Connected to ${mongoUri}.`)

  await seedData()
  xLogger.info(`Seed data for the database.`)

  await initServer()
}

run().catch(error => xLogger.error(error.stack))
