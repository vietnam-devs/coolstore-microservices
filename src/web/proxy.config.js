var urls = {
  web: existedOrDefault(process.env.NODE_WEB_ENV, 'http://localhost:8084/'),
  idp: existedOrDefault(process.env.NODE_IDP_ENV, 'http://localhost:8083/'),
  openapi: existedOrDefault(process.env.NODE_OPENAPI_ENV, 'http://localhost:5012/')
}

var host = existedOrDefault(process.env.NODE_WEB_ENV, 'http://localhost:8084/')
if (process.browser) {
  host = window.location.hostname
}

console.info(urls)

const PROXY_CONFIG = {
  baseUrl: host,
  idpUrl: `${urls['idp']}`,
  spaUrl: `${urls['web']}`,
  '/catalog/api/*': {
    target: `${urls['openapi']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
  },
  '/rating/api/*': {
    target: `${urls['openapi']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
  },
  '/cart/api/*': {
    target: `${urls['openapi']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
  },
  '/inventory/api/*': {
    target: `${urls['openapi']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
  },
  '/config': {
    target: `${urls['idp']}.well-known/openid-configuration`,
    secure: false,
    logLevel: 'debug',
    ignorePath: true,
    pathRewrite: { '^/config': '' }
  },
  '/.well-known/openid-configuration/jwks': {
    target: `${urls['idp']}.well-known/openid-configuration/jwks`,
    secure: false,
    logLevel: 'debug',
    ignorePath: true
  },
  '/host/*': {
    target: `${urls['idp']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true,
    pathRewrite: { '^/host': '' }
  },
  '/connect/*': {
    target: `${urls['idp']}`,
    secure: true,
    logLevel: 'debug',
    changeOrigin: true,
    router: function(req) {
      return url
    }
  }
}

function existedOrDefault(envVariable, defaultOne) {
  if (typeof envVariable !== 'undefined') {
    return envVariable
  } else {
    return defaultOne
  }
}

console.log(PROXY_CONFIG.spaUrl)
module.exports = PROXY_CONFIG
