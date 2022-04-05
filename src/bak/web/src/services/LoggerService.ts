enum LogLevel {
  NONE = 0,
  ERROR = 1,
  WARN = 2,
  INFO = 3,
  DEBUG = 4
}

class LoggerService {
  readonly _logger = console
  readonly _level: LogLevel

  constructor(logLevel: LogLevel) {
    this._level = logLevel
  }

  public debug(...msg: any[]) {
    if (this._level >= LogLevel.DEBUG) {
      this._logger.debug('DEBUG :', ...msg)
    }
  }

  public info(...msg: any[]) {
    if (this._level >= LogLevel.INFO) {
      this._logger.debug('INFO :', ...msg)
    }
  }

  public warn(...msg: any[]) {
    if (this._level >= LogLevel.WARN) {
      this._logger.debug('WARN :', ...msg)
    }
  }

  public error(...msg: any[]) {
    if (this._level >= LogLevel.ERROR) {
      this._logger.debug('ERROR :', ...msg)
    }
  }
}

export default new LoggerService(LogLevel.DEBUG)
