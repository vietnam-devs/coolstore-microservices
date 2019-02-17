import { default as logger, SimpleLogger as xLogger } from './logger'
import { RatingModel } from '../models/rating'
import { RatingService } from './rating-service'
const RatingData = require('./ratings.json')

export const seedData = async () => {
  const ratings = (await RatingService.findRatings()) || []

  if (ratings.length <= 0) {
    await RatingData.map(async (model: RatingModel) => {
      logger.info(model)
      await RatingService.createRating(model)
    })
    xLogger.info(`Seed data to database #${JSON.stringify(RatingData)} in current database count is #${ratings.length}`)
  }
}
