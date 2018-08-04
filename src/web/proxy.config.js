var url = 'http://localhost:5000/'
var urlSpa = 'http://localhost:8080/'
var urlIdp = 'http://localhost:5001/'
var urlCat = 'http://localhost:5002'
var urlCart = 'http://localhost:5002'
var urlInv = 'http://localhost:5004'
var urlRat = 'http://localhost:5007'

const env = process.env.NODE_ENV
const config = {
    mode: env || 'development'
}
if (config.mode == 'development') {
    urlSpa = "http://coolstore.local/"
    urlIdp = "http://id.coolstore.local/"
    url = "http://api.coolstore.local/"

    var urlCat = 'http://api.coolstore.local/cat/'
    var urlCart = 'http://api.coolstore.local/cat/'
    var urlInv = 'http://api.coolstore.local/cat/'
    var urlRat = 'http://api.coolstore.local/cat/'
}


const PROXY_CONFIG = {
    baseUrl: url,
    idpUrl: urlIdp,
    spaUrl: urlSpa,
    '/api/*': {
        target: url,
        secure: false,
        logLevel: 'debug'
    },
    '/cat/api/*': {
        target: urlCat,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/cat': '' }
    },
    '/rat/api/*': {
        target: urlRat,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/rat': '' }
    },
    '/cart/api/*': {
        target: urlCart,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/cart': '' }
    },
    '/inv/api/*': {
        target: urlInv,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/inv': '' }
    },
    '/config': {
        target: `${urlIdp}.well-known/openid-configuration`,
        secure: false,
        logLevel: 'debug',
        ignorePath: true,
        changeOrigin: true,
        pathRewrite: { '^/config': '' }
    },
    '/.well-known/openid-configuration/jwks': {
        target: `${urlIdp}.well-known/openid-configuration/jwks`,
        secure: false,
        logLevel: 'debug',
        ignorePath: true
    },
    '/host/*': {
        target: urlIdp,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/host': '' }
    },
    '/connect/*': {
        target: urlIdp,
        secure: true,
        logLevel: 'debug',
        changeOrigin: true,
        router: function (req) {
            return url
        }
    }
}

module.exports = PROXY_CONFIG
