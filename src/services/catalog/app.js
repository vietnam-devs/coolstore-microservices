const express = require('express')
const mongoose = require('mongoose')
const bodyParser = require('body-parser')
require('./models/product')

const Product = mongoose.model('Product')
const app = express()

var isProduction = process.env.NODE_ENV === 'production'
console.info(`Production environment is ${isProduction}`)

// Normal express config defaults
app.use(require('morgan')('dev'))
app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())

var mongoDbUri = 'mongodb://localhost:27017/catalog'
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
app.use(function(err, req, res, next) {
  res.status(err.status || 500)
  res.json({
    errors: {
      message: err.message,
      error: {}
    }
  })
})

app.get('/', function(req, res) {
  res.send('Catalog Service.')
})

app.get('/api/v1/products', async (req, res) => {
  var products = await Product.findProducts()
  res.send(products)
})

app.post('/api/v1/products', async (req, res) => {
  console.info(req.body)
  var newProduct = new Product()
  res.send(await newProduct.createProduct(req.body))
})

app.get('/api/v1/products/:productId', async (req, res) => {
  console.info(req.params)
  var product = await Product.findProduct(req.params.productId)
  res.send(product)
})

app.get('/healthz', (req, res) => {
  res.send({
    status: 'Healthy!'
  })
})

/// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error('Not Found')
  err.status = 404
  next(err)
})

// development error handler
// will print stacktrace
if (!isProduction) {
  app.use(function(err, req, res, next) {
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
  var server = app.listen(process.env.PORT || 5002, () => {
    console.info(`App's running at http://localhost:${server.address().port}`)
    console.info('Press CTRL-C to stop\n')
  })
}

module.exports = app
