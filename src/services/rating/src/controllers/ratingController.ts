declare var require: any

var uuid = require('uuid');
import { Route, Get, Post, Put, Body } from 'tsoa'
import { default as Rating, RatingCreateRequest, RatingUpdateRequest } from '../models/rating'

@Route(`api/ratings`)
export class RatingController {
  /**
   * Get the all ratings
   */
  @Get()
  public async GetAll(): Promise<any> {
    // @ts-ignore
    var ratings = Rating.find({}).exec()

    var result = []
    ratings.reduce(function (res, value) {
      if (!res[value.productId]) {
        res[value.productId] = {
          productId: value.productId,
          cost: 0,
          userId: value.userId,
          id: value.Id,
          count: 0,
          totalCost: 0
        }
        result.push(res[value.productId])
      }
      res[value.productId].count += 1
      var nextCost = res[value.productId].totalCost += value.cost
      res[value.productId].cost = nextCost / res[value.productId].count
      return res
    }, {})
    return Promise.resolve(result.map(function (item) {
      delete item.count
      delete item.totalCost
      return item
    }))
  }

  /**
   * Get rating by Id
   * @param ratingId rating Id
   */
  @Get(`{productId}`)
  public GetRatingByProductId(productId: string): Promise<any> {
    // @ts-ignore
    var modelReponse = {
      productId: productId,
      cost: 0
    }

    let ratingolds = Rating.findOne({ productId: productId }).exec()
    if (ratingolds.length == 0) {
      modelReponse.cost = 0
    } else {
      modelReponse.cost =
        ratingolds.reduce(
          (accumulator, currentValue) => accumulator + currentValue.cost, 0
        ) / ratingolds.length
    }
    return Promise.resolve(modelReponse)
  }

  /**
   * Create a rating
   * @param request This is a rating creation request description
   */
  @Post()
  public Create(@Body() request: RatingCreateRequest): Promise<any> {
    let rating = new Rating({ _id: uuid.v1(), ...request })
    console.log(rating)
    let result = Rating.create(rating)
    return Promise.resolve(result)
  }

  /**
   * Update a rating
   * @param request This is a rating update request description
   */
  @Put()
  public Update(@Body() request: RatingUpdateRequest): Promise<any> {
    let rating = Rating.findOne({ id: request.id }).exec()
    if (!rating) return null
    rating = { rating, ...request }
    console.log(rating)
    let result = Rating.update(rating)
    return Promise.resolve(result)
  }
}
