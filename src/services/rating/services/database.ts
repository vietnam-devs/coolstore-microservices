import * as EventEmitter from 'events'
import * as mongoose from 'mongoose'

import { default as Logger } from './logger'
import { RatingModel } from '../models/rating'
import { RatingService } from './rating'
const RatingData = require('./ratings.json')

const eventEmitter = new EventEmitter()
const mongoUri = `${process.env.MONGO_DB_URL || 'mongodb://localhost/rating'}`
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
  const ratings = (await RatingService.findRatings()) || []
  if (ratings.length == 0) {
    Logger.info(
      `Seed data to database #${JSON.stringify(RatingData)} in current database count is #${ratings.length}`
    )
    RatingData.map(async (model: RatingModel) => {
      Logger.info(model)
      await RatingService.createRating(model)
    })
  }

  eventEmitter.emit('ready')
})

export { eventEmitter }
