// Reference at https://dzone.com/articles/kubernetes-and-mean-stack-for-microservices-develo

var express = require('express')
var app = express()

app.get('/', function (req, res) {
  res.send("Catalog Service.")
})

/*app.get('/products', function (req, res) {
      var MongoClient = require('mongodb').MongoClient;
      var dbURL = 'mongodb://catalog-db/catalog';
      if (process.env.CATALOG_DB_SERVICE_HOST !== undefined) {
        dbURL = 'mongodb://' + process.env.CATALOG_DB_SERVICE_HOST + ':' + process.env.CATALOG_DB_SERVICE_PORT + '/catalog';
      }

      console.log("In Products. Calling MongoDB: " + dbURL);

      MongoClient.connect(dbURL, function (err, db) {

        try {
          if (err) throw err

          db.collection('products').find().toArray(function (err, result) {
            if (err) throw err

            productList = result;
            console.log(productList);

            res.send(productList);
          })

        } catch (ex) {
          console.error('Exception: /products');
          console.error(ex);

          res.send(productList);
        }
      });*/

app.get('/api/v1/products', function (req, res) {
  res.send([
    {
      id: 'ba16da71-c7dd-4eac-9ead-5c2c2244e69f',
      name: 'IPhone 8',
      desc: 'IPhone 8',
      price: 900
    },
    {
      id: '13d02035-2286-4055-ad2d-6855a60efbbb',
      name: 'IPhone X',
      desc: 'IPhone X',
      price: 1000
    }
  ])
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

app.listen(5002, () => {
  console.log('App is running at http://localhost:5002')
  console.log('Press CTRL-C to stop\n')
})

module.exports = app
