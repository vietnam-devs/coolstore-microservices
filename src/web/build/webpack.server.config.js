const webpack = require('webpack')
const merge = require('webpack-merge')
const base = require('./webpack.base.config')
const nodeExternals = require('webpack-node-externals')
const VueSSRServerPlugin = require('vue-server-renderer/server-plugin')

module.exports = merge(base, {
  target: 'node',
  devtool: '#source-map',
  entry: './src/entry-server.js',
  output: {
    filename: 'server-bundle.js',
    libraryTarget: 'commonjs2'
  },
  resolve: {
    alias: {
      'create-api': './create-api-server.js'
    }
  },
  // https://webpack.js.org/configuration/externals/#externals
  // https://github.com/liady/webpack-node-externals
  externals: nodeExternals({
    // do not externalize CSS files in case we need to import it from a dep
    whitelist: ['buefy', /\.css$/]
  }),
  plugins: [
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
    new VueSSRServerPlugin()
  ]
})
