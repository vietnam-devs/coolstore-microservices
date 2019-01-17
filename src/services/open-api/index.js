const express = require('express')
const app = express()
const swaggerUi = require('swagger-ui-express')
const swaggerDocument = require('./apidocs.swagger.json')
const port = 5010

app.use('/', swaggerUi.serve, swaggerUi.setup(swaggerDocument))
app.listen(port, '0.0.0.0', () => console.log(`OpenAPI app listening on port ${port}!`))
