var urls = {
  web: process.env.NODE_WEB_ENV || 'http://localhost:8084/',
  idp: process.env.NODE_IDP_ENV || 'http://localhost:8083/',
  openapi: process.env.NODE_OPENAPI_ENV || 'http://localhost:5012/'
}

var host = urls['web']
if (process.browser) {
  host = window.location.hostname
}

console.info(urls)

const PROXY_CONFIG = {
  baseUrl: {
    target: host
  },
  idpUrl: { target: `${urls['idp']}` },
  spaUrl: {
    target: `${urls['web']}`
  },
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

console.log(PROXY_CONFIG.spaUrl.target)
module.exports = PROXY_CONFIG
