var url = "http://localhost:5000/"
var urlIdp = "http://localhost:5001/"
var urlSpa = "http://localhost:8080/"

const env = process.env.NODE_ENV
const config = {
   mode: env || 'development'
}

if(config.mode == "production"){
	urlSpa = process.env.WEB_HOST_ALIAS
	urlIdp = process.env.ID_HOST_ALIAS
	url = process.env.API_HOST_ALIAS
}

const PROXY_CONFIG = {
    baseUrl: url,
	idpUrl: urlIdp,
	spaUrl: urlSpa,
    "/api/*": {
        target: url,
        secure: false,
        logLevel: "debug",
    },
    "/config": {
        target: `${urlIdp}.well-known/openid-configuration`,
        secure: false,
        logLevel: "debug",
        ignorePath: true,
        changeOrigin: true,
        pathRewrite: { '^/config': '' },
    },
    "/.well-known/openid-configuration/jwks": {
        target: `${urlIdp}.well-known/openid-configuration/jwks`,
        secure: false,
        logLevel: "debug",
        ignorePath: true,
    },
    "/host/*": {
        target: urlIdp,
        secure: false,
        logLevel: "debug",
        changeOrigin: true,
        pathRewrite: { '^/host': '' },
    },
    "/connect/*": {
        target: urlIdp,
        secure: true,
        logLevel: "debug",
        changeOrigin: true,
        router: function (req) {
            return url;
        }
    }
}

module.exports = PROXY_CONFIG;