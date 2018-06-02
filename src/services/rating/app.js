var express = require('express');
var app = express();

app.get('/', function (req, res) {
  res.send("Hi. I'm Rating Service.");
});

app.get('/health', function (req, res) {
  res.send({
    status: 'Rating Service is healthy.'
  });
});

app.listen(3000, () => {
  console.log('App is running at http://localhost:3000');
  console.log('Press CTRL-C to stop\n');
});

module.exports = app;