const proxyConfig = require('../proxy.config.js')

export const oidcSettings = {
    authority: '/config',
    client_id: 'spa',
    redirect_uri: proxyConfig.spaUrl + 'callback',
    response_type: 'token id_token',
    automaticSilentRenew: true,
    silent_redirect_uri: proxyConfig.spaUrl + 'callback',
    scope: 'inventory_api_scope cart_api_scope pricing_api_scope review_api_scope catalog_api_scope openid profile',
}