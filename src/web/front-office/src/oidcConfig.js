const proxyConfig = require('../proxy.config.js')

export const oidcSettings = {
  authority: '/config',
  client_id: 'spa',
  redirect_uri: proxyConfig.spaUrl + 'callback',
  response_type: 'token id_token',
  automaticSilentRenew: true,
  post_logout_redirect_uri: proxyConfig.spaUrl,
  silent_redirect_uri: proxyConfig.spaUrl + 'callback',
  scope: 'inventory_api_scope cart_api_scope catalog_api_scope rating_api_scope openid profile'
}
