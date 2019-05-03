import * as Pino from 'pino'
import * as chalk from 'chalk'

export default Pino({
  name: 'catalog-service',
  messageKey: 'message',
  changeLevelName: 'severity',
  useLevelLabels: true
})

export const SimpleLogger = {
  info: (message: any) => {
    console.log(`${chalk.default.green('✓')} ${message}`)
  },
  error: (message: any) => {
    console.log(`${chalk.default.red('✗')} ${message}`)
  }
}
