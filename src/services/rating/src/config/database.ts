declare var require: any
declare var process: any

let mongoose = require('mongoose')
import { default as Rating } from '../models/rating'
let ratingsData = require('./ratings.json')

  // @ts-ignore: ignore to check global variables
  ; (mongoose as any).Promise = global.Promise

let isProduction = process.env.NODE_ENV === 'production'
console.info(`Production environment is ${isProduction}`)

let mongoDbUri = process.env.MONGO_DB_URL || 'mongodb://localhost:27017/rating'

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
  var db = mongoose.connection;
  db.once('open', function callback() {
    console.info("First time connect to database")
    Rating.find({}, function (err, ratings) {
      ratings = ratings || []
      if (ratings.length == 0) {
        console.info(`Seed data to database #${JSON.stringify(ratingsData)} in current database count is #${ratings.length}`)
        ratingsData.map(product => {
          Rating.create(product, function (err) {
            if (err) console.info(err)
          })
        })
      }
    })
  });
}

export { mongoose, mongoDbUri }
