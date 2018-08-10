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
console.log(`Base path is ${basePath}`)

app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())
app.use(methodOverride())

RegisterRoutes(app, basePath)

app.use('/swagger.json', express.static(__dirname + './swagger.json'))

app.use('/swagger', swaggerUI.serve, swaggerUI.setup(swaggerJSON))

app.listen(process.env.PORT || 5002, () => {
  console.info(`App's running at http://localhost:${process.env.PORT || 5002}`)
  console.info('Press CTRL-C to stop\n')
})
