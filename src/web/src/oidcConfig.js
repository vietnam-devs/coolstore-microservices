export const oidcSettings = {
  authority: '/config',
  client_id: 'spa',
  redirect_uri: 'http://localhost:8080/callback',
  response_type: 'token',
  scope: 'inventory_api_scope cart_api_scope pricing_api_scope review_api_scope catalog_api_scope'
}