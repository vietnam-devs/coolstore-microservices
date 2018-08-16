declare var require: any
declare var process: any
declare var __dirname: any

let express = require('express')
let methodOverride = require('method-override')

import * as bodyParser from 'body-parser'
import swaggerUI from 'swagger-ui-express'

import { mongoose, mongoDbUri } from './config/database'
import { RegisterRoutes } from './routes'
const app = express()

let basePath = process.env.BASE_PATH
if (!basePath) {
  basePath = '/'
}
console.info(`Base path is ${basePath}`)

const isProduction = process.env.NODE_ENV === 'production'
const swaggerJSON = isProduction ? require('./swagger.json') : require('../dist/swagger.json')

app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())
app.use(methodOverride())

RegisterRoutes(app)
app.get(`/`, (req, res) => {
  res.redirect(`${basePath}swagger`)
})
app.use(`${basePath}healthz`, (_, res) => {
  res.send({
    status: 'Healthy!'
  })
})
app.use(`${basePath}swagger.json`, express.static(__dirname + './swagger.json'))
app.use(`${basePath}swagger`, swaggerUI.serve, swaggerUI.setup(swaggerJSON))

function startServer() {
  app.listen(process.env.PORT || 5002, () => {
    console.info(`App's running at http://localhost:${process.env.PORT || 5002}`)
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
