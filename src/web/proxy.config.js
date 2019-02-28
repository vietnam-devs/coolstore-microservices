const env = process.env.NODE_ENV
const config = {
  mode: env || 'development'
}

var urls = {
  web: process.env.NODE_WEB_ENV || 'http://localhost:8080/',
  idp: process.env.NODE_IDP_ENV || 'http://localhost:5001/',
  idpHost: process.env.NODE_IDP_HOST || 'http://localhost:5001/',
  catalog: process.env.NODE_CATALOG_ENV || 'http://localhost:8082/',
  cart: process.env.NODE_CART_ENV || 'http://localhost:8082/',
  inventory: process.env.NODE_INVENTORY_ENV || 'http://localhost:8082/',
  rating: process.env.NODE_RATING_ENV || 'http://localhost:8082/'
}

var host = 'http://localhost:8080/'

if (process.browser) {
  host = window.location.hostname
}

console.info(urls)

const PROXY_CONFIG = {
  baseUrl: host,
  idpUrl: `${urls['idp']}`,
  spaUrl: `${urls['web']}`,
  '/catalog/api/*': {
    target: `${urls['catalog']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
    //pathRewrite: { '^/catalog': '' }
  },
  '/rating/api/*': {
    target: `${urls['rating']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
    //pathRewrite: { '^/rating': '' }
  },
  '/cart/api/*': {
    target: `${urls['cart']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
    //pathRewrite: { '^/cart': '' }
  },
  '/inventory/api/*': {
    target: `${urls['inventory']}`,
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
    //pathRewrite: { '^/inventory': '' }
  },
  '/config': {
    target: `${urls['idp']}.well-known/openid-configuration`,
    secure: false,
    logLevel: 'debug',
    ignorePath: true,
    headers: { Host: `${urls['idpHost']}` }
    //pathRewrite: { '^/config': '' }
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
console.log(PROXY_CONFIG.spaUrl)
module.exports = PROXY_CONFIG
