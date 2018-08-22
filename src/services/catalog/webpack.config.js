var path = require('path')

var fs = require('fs')
var nodeModules = {}
fs.readdirSync('node_modules')
  .filter(function (x) {
    return ['.bin'].indexOf(x) === -1
  })
  .forEach(function (mod) {
    nodeModules[mod] = 'commonjs ' + mod
  })

module.exports = {
  entry: './src/server.ts',
  devtool: 'inline-source-map',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'bundle.js'
  },
  plugins: [],
  resolve: {
    modules: [__dirname, 'node_modules'],
    extensions: [ '.ts', '.tsx', ".js", ".json"]
  },
  node: {
    fs: 'empty'
  },
  module: {
    rules: [
      {
        use: 'tslint-loader',
        test: /\.ts?$/
      }
    ]
  },
  target: 'node',
  externals: nodeModules
}
