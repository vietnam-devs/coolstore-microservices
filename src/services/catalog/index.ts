require('dotenv').config()
import initServer from './services/grpc-server'
import { ProductService } from './services/product-service'
import { default as logger, SimpleLogger as xLogger } from './services/logger'

ProductService.initDb().then(
  async () => {
    xLogger.error("Begin start server")
    await initServer()
  },
  error => xLogger.error(error.stack)
)
