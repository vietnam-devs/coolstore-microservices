var url = 'http://localhost:5000/'
var urlIdp = 'http://localhost:5001/'
var urlSpa = 'http://localhost:8080/'

const env = process.env.NODE_ENV
const config = {
    mode: env || 'development'
}
console.log("Enviroment")
console.log(process.env.NODE_ENV)
console.log("web host")
console.log(process.env.WEB_HOST_ALIAS)
if (config.mode == 'production') {
    urlSpa = `http://${process.env.WEB_HOST_ALIAS}/`
    urlIdp = `http://${process.env.ID_HOST_ALIAS}/`
    url = `http://${process.env.API_HOST_ALIAS}/`
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
        router: function(req) {
            return url
        }
    }
}

module.exports = PROXY_CONFIG
