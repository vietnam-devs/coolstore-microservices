import * as Pino from 'pino'

export default Pino({
  name: 'rating-service',
  messageKey: 'message',
  changeLevelName: 'severity',
  useLevelLabels: true
})
