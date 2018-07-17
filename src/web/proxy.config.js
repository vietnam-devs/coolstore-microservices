const url = "http://localhost:5000/";
const urlDpi = "http://localhost:5001/";

//Config proxy
var host = process.env.GATEWAY_SERVICE_SERVICE_HOST;
var port = process.env.GATEWAY_SERVICE_SERVICE_PORT;


if(!port){
	port = 80
}

if(host){
	url = `${host}:${port}/api`
}

const PROXY_CONFIG = {
    baseUrl: url,
    "/api/*": {
        target: url,
        secure: false,
        logLevel: "debug",
    },
    "/config": {
        target: `${urlDpi}/.well-known/openid-configuration`,
        secure: false,
        logLevel: "debug",
        ignorePath: true,
        changeOrigin: true,
        pathRewrite: { '^/config': '' },
        router: function (req) {
            return `${urlDpi}/.well-known/openid-configuration`;
        }
    },
    "/.well-known/openid-configuration/jwks": {
        target: `${urlDpi}/.well-known/openid-configuration/jwks`,
        secure: false,
        logLevel: "debug",
        ignorePath: true,
    },
    "/host/*": {
        target: urlDpi,
        secure: false,
        logLevel: "debug",
        changeOrigin: true,
        pathRewrite: { '^/host': '' },
    },
    "/connect/*": {
        target: urlDpi,
        secure: true,
        logLevel: "debug",
        changeOrigin: true,
        router: function (req) {
            return url;
        }
    }
}

module.exports = PROXY_CONFIG;