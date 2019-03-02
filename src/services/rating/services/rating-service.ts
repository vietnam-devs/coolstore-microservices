import * as uuid from 'uuid'
import { default as Logger } from './logger'
import { default as ratingSchema, RatingModel } from '../models/rating'

export class RatingService {
  static async findRatingByProductId(productId: string) {
    return await ratingSchema.findOne({ productId: productId }).exec()
  }
  static async findRatings() {
    return await ratingSchema.find({}).exec()
  }
  static async createRating(model: RatingModel) {
    const id = uuid.v1()
    Logger.info({ ...model, _id: id })
    return await new ratingSchema({ ...model, _id: id }).save()
  }
  static async updateRating(model: RatingModel) {
    return await new ratingSchema().update({ id: model.id }, { ...model })
  }
}
