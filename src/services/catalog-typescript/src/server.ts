import * as bodyParser from 'body-parser'
import * as express from 'express'
import * as methodOverride from 'method-override'
import * as swaggerUI from 'swagger-ui-express'

import { mongoose, mongoDbUri } from './config/database'
import Router from './routes'
const app = express()

let basePath = process.env.BASE_PATH
if (!basePath) {
  basePath = '/'
}
console.info(`Base path is ${basePath}`)

const isProduction = process.env.NODE_ENV === 'production'
const swaggerJSON = isProduction
  ? require('./swagger.json')
  : require('../dist/swagger.json')

app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())
app.use(methodOverride())

app.use(`${basePath}api/products`, Router)
app.use(`${basePath}healthz`, (req, res) => {
  res.send({
    status: 'Healthy!'
  })
})
app.use(`${basePath}swagger.json`, express.static(__dirname + './swagger.json'))
app.use(`${basePath}swagger`, swaggerUI.serve, swaggerUI.setup(swaggerJSON))

function startServer() {
  app.listen(process.env.PORT || 5002, () => {
    console.info(
      `App's running at http://localhost:${process.env.PORT || 5002}`
    )
    console.info('Press CTRL-C to stop\n')
  })
}

mongoose.connection.once('open', function() {
  console.info(`Connected to ${mongoDbUri}.`)
  app.emit('ready')
})

app.on('ready', function() {
  startServer()
})
