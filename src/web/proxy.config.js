var url = 'http://localhost:5000/'
var urlIdp = 'http://localhost:5001/'
var urlSpa = 'http://localhost:8080/'

const env = process.env.NODE_ENV
const config = {
    mode: env || 'development'
}
if (config.mode == 'production') {
    urlSpa = "http://coolstore.local/"
    urlIdp = "http://id.coolstore.local/"
    url = "http://api.coolstore.local/"
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
