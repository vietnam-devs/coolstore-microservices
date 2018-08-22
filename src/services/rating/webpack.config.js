'use strict'

var path = require('path')
const WebpackShellPlugin = require('webpack-shell-plugin')

var fs = require('fs')
var nodeModules = {}
fs.readdirSync(path.join(__dirname, './node_modules'))
  .filter(function(x) {
    return ['.bin'].indexOf(x) === -1
  })
  .forEach(function(mod) {
    nodeModules[mod] = 'commonjs ' + mod
  })

module.exports = {
  entry: './src/server.ts',
  devtool: 'inline-source-map',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'bundle.js'
  },
  plugins: [
    new WebpackShellPlugin({
      onBuildStart: ['npm run swagger-gen && npm run routes-gen']
    })
  ],
  resolve: {
    modules: [path.join(__dirname, './src'), path.join(__dirname, './node_modules')],
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
  target: 'node',
  externals: nodeModules
}
