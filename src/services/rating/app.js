const express = require('express')
const mongoose = require('mongoose')
const bodyParser = require('body-parser')
require('./models/rating')
const swaggerUI = require('swagger-ui-express')
const swaggerJSON = require('./swagger/swagger.json')

const Rating = mongoose.model('Rating')
const app = express()

var isProduction = process.env.NODE_ENV === 'production'
console.info(`Production environment is ${isProduction}`)

var basePath = process.env.BASE_PATH
if (!basePath) {
  basePath = '/'
}
console.info(`Base path is ${basePath}`)

// Normal express config defaults
app.use(require('morgan')('dev'))
app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())

var mongoDbUri = 'mongodb://localhost:27017/rating'
mongoose.set('debug', true)
if (isProduction) {
  mongoDbUri = process.env.MONGO_DB_URL
}

function connect() {
  console.info(`MongoDB's running at ${mongoDbUri}`)
  return mongoose.connect(
    mongoDbUri,
    {
      useNewUrlParser: true,
      keepAlive: 120
    }
  )
}

// start to connect to database
connect()
  .then(() => {
    startServer()
  })
  .catch(err => {
    console.error('Error on start: ' + err.stack)
    process.exit(1)
  })

// production error handler
// no stacktraces leaked to user
app.use(function (err, req, res, next) {
  res.status(err.status || 500)
  res.json({
    errors: {
      message: err.message,
      error: {}
    }
  })
})

app.get(`${basePath}api/ratings`, async (req, res) => {
  console.info(req.body)
  var rattings = await Rating.findRatings()
  var result = []
  rattings.reduce(function (res, value) {
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
  res.send(result.map(function (item) {
    delete item.count
    delete item.totalCost
    return item
  }))
})

app.get(`${basePath}api/ratings/:productId`, async (req, res) => {
  console.info(req.body)
  var productId = req.params.productId
  var modelReponse = {
    productId: productId,
    cost: 0
  }
  var ratingolds = await Rating.getRatingByProductId(productId)
  if (ratingolds.length == 0) {
    modelReponse.cost = 0
  } else {
    modelReponse.cost =
      ratingolds.reduce(
        (accumulator, currentValue) => accumulator + currentValue.cost, 0
      ) / ratingolds.length
  }
  res.send(modelReponse)
})

app.post(`${basePath}api/ratings`, async (req, res) => {
  console.info(req.body)
  var newRating = new Rating()
  var rating = newRating.createRating(req.body)
  await rating.save(function (err) {
    if (err) {
      res.status(400).send({ error: err })
    } else {
      res.send(rating)
    }
  })
})

app.put(`${basePath}api/ratings`, async (req, res) => {
  console.info(req.body)
  var rating = await Rating.findRatingByProductIdAndUserId(
    req.body.productId,
    req.body.userId
  )
  rating.cost = req.body.cost
  await rating.save(function (err) {
    if (err) {
      res.status(400).send({ error: err })
    } else {
      res.send(rating)
    }
  })
})

app.get(`${basePath}healthz`, (req, res) => {
  res.send({
    status: 'Healthy!'
  })
})

app.get(`${basePath}`, function (req, res) {
  res.send('Rating Service.')
})

app.use(`${basePath}swagger`, swaggerUI.serve, swaggerUI.setup(swaggerJSON));

/// catch 404 and forward to error handler
app.use(function (req, res, next) {
  var err = new Error('Not Found')
  err.status = 404
  next(err)
})

// development error handler
// will print stacktrace
if (!isProduction) {
  app.use(function (err, req, res, next) {
    console.error(err.stack)
    res.status(err.status || 500)
    res.json({
      errors: {
        message: err.message,
        error: err
      }
    })
  })
}

function startServer() {
  var server = app.listen(process.env.PORT || 5007, () => {
    console.info(`App's running at http://localhost:${server.address().port}${basePath}`)
    console.info('Press CTRL-C to stop\n')
  })
}

function groupBy(xs, key) {
  return xs.reduce(function (rv, x) {
    (rv[x[key]] = rv[x[key]] || []).push(x)
    return rv
  }, {})
}

module.exports = app
