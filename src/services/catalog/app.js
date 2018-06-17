// Reference at https://dzone.com/articles/kubernetes-and-mean-stack-for-microservices-develo

var express = require('express');
var app = express();

app.get('/', function (req, res) {
  res.send("Hi. I'm Catalog Service.");
});

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

app.post('/products', function (req, res) {

});

app.get('/health', function (req, res) {
  res.send({
    status: 'Catalog Service is healthy.'
  });
});

app.listen(3000, () => {
  console.log('App is running at http://localhost:3000');
  console.log('Press CTRL-C to stop\n');
});

module.exports = app;
