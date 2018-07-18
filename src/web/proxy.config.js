var url = "http://localhost:5000/";
var urlIdp = "http://localhost:5001/";
var urlSpa = "http://localhost:8080/"

//Config proxy
var host = process.env.GATEWAY_SERVICE_SERVICE_HOST;
var port = process.env.GATEWAY_SERVICE_SERVICE_PORT;

var hostIdp = process.env.IDP_SERVICE_SERVICE_HOST;
var portIdp = process.env.IDP_SERVICE_SERVICE_PORT;

var hostSpa = process.env.SPA_SERVICE_SERVICE_HOST;
var portSpa = process.env.SPA_SERVICE_SERVICE_PORT;

if(!port){
	port = 80
}

if(host){
	url = `${host}:${port}/api`
}

if(!portIdp){
	portIdp = 80
}

if(host){
	urlIdp = `${hostIdp}:${portIdp}/`
}

if(!portSpa){
	portSpa = 80
}

if(hostSpa){
	urlSpa = `${hostSpa}:${portSpa}/`
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
        target: `${urlIdp}/.well-known/openid-configuration`,
        secure: false,
        logLevel: "debug",
        ignorePath: true,
        changeOrigin: true,
        pathRewrite: { '^/config': '' },
        router: function (req) {
            return `${urlIdp}/.well-known/openid-configuration`;
        }
    },
    "/.well-known/openid-configuration/jwks": {
        target: `${urlIdp}/.well-known/openid-configuration/jwks`,
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