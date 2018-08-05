const uuid = require('uuid/v1')
const mongoose = require('mongoose')

const Schema = mongoose.Schema

var RatingSchema = new Schema({
  _id: { type: String, default: uuid },
  productId: { type: String },
  userId: { type: String },
  cost: { type: Number }
})

// Duplicate the ID field.
RatingSchema.virtual('id').get(function() {
  return this._id
})

// Ensure virtual fields are serialised.
RatingSchema.set('toJSON', {
  virtuals: true
})

RatingSchema.path('productId').required(true, 'Product Id cannot be blank')
RatingSchema.path('userId').required(true, 'UserId cannot be blank')
RatingSchema.path('cost').validate(function(price) {
  // https://gist.github.com/rutcreate/03ff3f9bd5f414465322
  return Number(price).toString() === price.toString()
}, 'Price must be a float number.')

RatingSchema.methods = {
  createRating: function(rating) {
    this._id = uuid()
    this.productId = rating.productId
    this.userId = rating.userId
    this.cost = rating.cost
    return this
  },
  updateCostRating: function(rating, cost) {
    this.getRatingByProductIdAndUserid(rating.productId, rating.userId).then(
      (error, data) => {
        data.cost = rating.cost
        return data
      }
    )
  }
}

RatingSchema.statics = {
  findRating: function(_id) {
    return this.find({ _id }).exec()
  },
  findRatings: function(options) {
    return this.find({}).exec()
  },
  findRatingByProductId: function(productId) {
    return this.find({ productId: productId }).exec()
  },
  getRatingByProductId: function(productId) {
    return this.find({ productId: productId }).exec()
  },
  getRatingByProductIdAndUserid: function(productId, userId) {
    return this.findOne({ productId: productId, userId: userId }).exec()
  },
  findRatingByProductIdAndUserId: function(productId, userId) {
    return this.findOne({ productId: productId, userId: userId }).exec()
  }
}

mongoose.model('Rating', RatingSchema)
