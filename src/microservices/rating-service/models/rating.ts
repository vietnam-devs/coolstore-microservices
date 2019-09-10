import { Schema, model } from 'mongoose'

export interface RatingModel {
  id: string
  productId: string
  userId: string
  cost: number
}

let ratingSchema = new Schema({
  _id: {
    type: String,
    required: `Enter an guid id.`
  },
  productId: {
    type: String,
    required: `Product Id can't be blank.`
  },
  userId: {
    type: String,
    required: `User id can't be blank.`
  },
  cost: {
    type: Number,
    required: `Cost can't be blank.`
  }
})

// Duplicate the ID field.
ratingSchema.virtual('id').get(function() {
  return this._id
})

// Ensure virtual fields are serialised.
ratingSchema.set('toJSON', {
  virtuals: true
})

ratingSchema.path('productId').required(true, `Product id can't be blank.`)
ratingSchema.path('userId').required(true, `User id can't be blank.`)
ratingSchema.path('cost').validate(function(price) {
  return Number(price).toString() === price.toString()
}, `Cost must be a float number.`)

export default model('Rating', ratingSchema)
