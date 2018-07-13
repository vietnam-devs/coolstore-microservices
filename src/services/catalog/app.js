// Reference at https://dzone.com/articles/kubernetes-and-mean-stack-for-microservices-develo

const express = require('express')
const app = express()

const uuid = require('uuid/v1')
const mongoose = require('mongoose')
const Schema = mongoose.Schema;

var ProductSchema = new Schema({
  _id: { type: String, default: uuid },
  name: { type: String },
  desc: { type: String },
  price: { type: Number }
})

ProductSchema.statics = {
  findProducts: function (options) {
    return this.find({}).exec()
  }
}

mongoose.model('Product', ProductSchema)

app.get('/', function (req, res) {
  res.send("Catalog Service.")
})

app.get('/api/v1/products', async (req, res) => {
  var uri = process.env.MONGO_DB_URL
  console.log(uri)

  mongoose.connect(uri, { useNewUrlParser: true })

  const Product = mongoose.model('Product')

  var newProduct = new Product()
  newProduct._id = uuid()
  newProduct.name = "name 1"
  newProduct.desc = "desc 1"
  newProduct.price = 100
  newProduct.save()

  var products = await Product.findProducts()
  res.send(products)
})

app.get('/api/v1/products/:productId', function (req, res) {
  res.send({
    id: 'ba16da71-c7dd-4eac-9ead-5c2c2244e69f',
    name: 'IPhone 8',
    desc: 'IPhone 8',
    price: 900
  })
})

app.post('/api/v1/products', function (req, res) {

  res.send(req.body)
})

app.get('/healthz', function (req, res) {
  res.send({
    status: 'Healthy!'
  })
})

process.on('unhandledRejection', (reason, promise) => {
  console.log('Unhandled Rejection at:', reason.stack || reason)
  // Recommended: send the information to sentry.io
  // or whatever crash reporting service you use
})

app.listen(5002, () => {
  console.log('App is running at http://localhost:5002')
  console.log('Press CTRL-C to stop\n')
})

module.exports = app
