export const oidcSettings = {
  authority: 'http://localhost:5001',
  client_id: 'spa',
  redirect_uri: 'http://localhost:8080/callback',
  response_type: 'token',
  scope: 'inventory_api_scope cart_api_scope pricing_api_scope review_api_scope catalog_api_scope'
}