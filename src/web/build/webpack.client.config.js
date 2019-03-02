const webpack = require('webpack')
const merge = require('webpack-merge')
const base = require('./webpack.base.config')
const SWPrecachePlugin = require('sw-precache-webpack-plugin')
const VueSSRClientPlugin = require('vue-server-renderer/client-plugin')

const config = merge(base, {
  entry: {
    app: './src/entry-client.js'
  },
  resolve: {
    alias: {
      'create-api': './create-api-client.js'
    }
  },
  plugins: [
    // strip dev-only code in Vue source
    new webpack.DefinePlugin({
      'process.env.NODE_ENV': JSON.stringify(`${process.env.NODE_ENV}` || 'development'),
      'process.env.NODE_WEB_ENV': JSON.stringify(`${process.env.NODE_WEB_ENV}` || 'http://localhost:8080/'),
      'process.env.NODE_IDP_ENV': JSON.stringify(`${process.env.NODE_IDP_ENV}` || 'http://localhost:5001'),
      'process.env.NODE_IDP_HOST': JSON.stringify(`${process.env.NODE_IDP_HOST}` || 'localhost:5001'),
      'process.env.NODE_CATALOG_ENV': JSON.stringify(`${process.env.NODE_CATALOG_ENV}` || 'http://localhost:8082/'),
      'process.env.NODE_CART_ENV': JSON.stringify(`${process.env.NODE_CART_ENV}` || 'http://localhost:8082/'),
      'process.env.NODE_INVENTORY_ENV': JSON.stringify(`${process.env.NODE_INVENTORY_ENV}` || 'http://localhost:8082/'),
      'process.env.NODE_RATING_ENV': JSON.stringify(`${process.env.NODE_RATING_ENV}` || 'http://localhost:8082/'),
      'process.env.VUE_ENV': '"client"'
    }),
    // extract vendor chunks for better caching
    new webpack.optimize.CommonsChunkPlugin({
      name: 'vendor',
      minChunks: function(module) {
        // a module is extracted into the vendor chunk if...
        return (
          // it's inside node_modules
          /node_modules/.test(module.context) &&
          // and not a CSS file (due to extract-text-webpack-plugin limitation)
          !/\.css$/.test(module.request)
        )
      }
    }),
    // extract webpack runtime & manifest to avoid vendor chunk hash changing
    // on every build.
    new webpack.optimize.CommonsChunkPlugin({
      name: 'manifest'
    }),
    new VueSSRClientPlugin()
  ]
})

if (process.env.NODE_ENV === 'production') {
  config.plugins.push(
    // auto generate service worker
    new SWPrecachePlugin({
      cacheId: 'vue-coolstore',
      filename: 'service-worker.js',
      minify: true,
      dontCacheBustUrlsMatching: /./,
      staticFileGlobsIgnorePatterns: [/\.map$/, /\.json$/],
      runtimeCaching: [
        {
          urlPattern: '/',
          handler: 'networkFirst'
        }
      ]
    })
  )
}

module.exports = config
