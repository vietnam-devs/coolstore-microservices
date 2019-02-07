require('dotenv').config()
import { eventEmitter } from './services/database'
import { default as initServer } from './services/grpc-server'

eventEmitter.on('ready', async () => {
  await initServer().catch(console.error)
})
