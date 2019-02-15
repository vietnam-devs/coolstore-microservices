require('dotenv').config()
import initDb from './services/database'
import initServer from './services/grpc-server'

initDb(async () => {
  await initServer().catch(console.error)
})
