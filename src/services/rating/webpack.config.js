'use strict'

var path = require('path')
const WebpackShellPlugin = require('webpack-shell-plugin')

var fs = require('fs')
var nodeModules = {}
fs.readdirSync(path.join(__dirname, './node_modules'))
  .filter(function (x) {
    return ['.bin'].indexOf(x) === -1
  })
  .forEach(function (mod) {
    nodeModules[mod] = 'commonjs ' + mod
  })

const PATHS = {
  src: path.join(__dirname, './src'),
  dist: path.join(__dirname, './dist'),
  modules: path.join(__dirname, './node_modules')
};

module.exports = {
  entry: './src/server.ts',
  devtool: 'source-map',
  output: {
    path: PATHS.dist,
    filename: 'bundle.js'
  },
  plugins: [
    new WebpackShellPlugin({
      onBuildStart: ['npm run swagger-gen && npm run routes-gen']
    })
  ],
  resolve: {
    modules: [PATHS.src, PATHS.modules],
    extensions: ['.ts']
  },
  node: {
    fs: 'empty'
  },
  module: {
    rules: [
      {
        use: 'ts-loader',
        test: /\.ts$/
      }
    ]
  },
  devServer: {
    contentBase: PATHS.dist,
    compress: true,
    headers: {
      'X-Content-Type-Options': 'nosniff',
      'X-Frame-Options': 'DENY'
    },
    open: true,
    overlay: {
      warnings: true,
      errors: true
    },
    port: 8080,
    publicPath: 'http://localhost:8080/',
    hot: true
  },
  target: 'node',
  externals: nodeModules
}
