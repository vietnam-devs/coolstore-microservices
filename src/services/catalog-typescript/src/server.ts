import * as bodyParser from 'body-parser'
import * as express from 'express'
import * as methodOverride from 'method-override'
import * as swaggerUI from 'swagger-ui-express'
import { RegisterRoutes } from './routes'
import './controllers/productsController'
import * as mongoose from 'mongoose'

const swaggerJSON = require('./swagger.json')
const app = express()

let basePath = process.env.BASE_PATH
if (!basePath) {
  basePath = '/'
}
console.info(`Base path is ${basePath}`)

let isProduction = process.env.NODE_ENV === 'production'
console.info(`Production environment is ${isProduction}`)

let mongoDbUri = 'mongodb://localhost:27017/catalog'
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
app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())
app.use(methodOverride())

RegisterRoutes(app, basePath)

app.use('/swagger.json', express.static(__dirname + './swagger.json'))

app.use('/swagger', swaggerUI.serve, swaggerUI.setup(swaggerJSON))

connect()
  .then(() => {
    startServer()
  })
  .catch(err => {
    console.error('Error on start: ' + err.stack)
    process.exit(1)
  })

function startServer() {
  app.listen(process.env.PORT || 5002, () => {
    console.info(`App's running at http://localhost:${process.env.PORT || 5002}`)
    console.info('Press CTRL-C to stop\n')
  })
}
