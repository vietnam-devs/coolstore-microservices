require('dotenv').config()
import initServer from './services/grpc-server'
//import { ProductService } from './services/product-service'
import { default as logger, SimpleLogger as xLogger } from './services/logger'

xLogger.error('Begin start server')
initServer()
