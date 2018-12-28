var urls = {
    web: 'http://localhost:8080/',
    idp: 'http://localhost:5001/',
    catalog: 'http://localhost:5002/',
    cart: 'http://localhost:5003/',
    inventory: 'http://localhost:5004/',
    rating: 'http://localhost:5007/'
}

const env = process.env.NODE_ENV
const config = {
    mode: env || 'development'
}
var host = 'coolstore.local'

if(process.browser){
	host = window.location.hostname
}

if (config.mode == 'production') {
    urls = {
        ...urls,
        ...{
            web: `http://${host}/`,
            idp: `http://id.${host}/`,
			api: `http://api.${host}/`,
            catalog: `http://api.${host}/catalog/`,
            cart: `http://api.${host}/cart/`,
            inventory: `http://api.${host}/inventory`,
            rating: `http://api.${host}/rating`,
		}
    }
}

console.info(urls)

const PROXY_CONFIG = {
    baseUrl: `${urls['web']}`,
    idpUrl: `${urls['idp']}`,
    spaUrl: `${urls['web']}`,
    '/catalog/api/*': {
        target: `${urls['catalog']}`,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/catalog': '' }
    },
    '/rating/api/*': {
        target: `${urls['rating']}`,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/rating': '' }
    },
    '/cart/api/*': {
        target: `${urls['cart']}`,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/cart': '' }
    },
    '/inventory/api/*': {
        target: `${urls['inventory']}`,
        secure: false,
        logLevel: 'debug',
        changeOrigin: true,
        pathRewrite: { '^/inventory': '' }
    },
    '/config': {
        target: `${urls['idp']}.well-known/openid-configuration`,
        secure: false,
        logLevel: 'debug',
        ignorePath: true,
        changeOrigin: true,
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

module.exports = PROXY_CONFIG
