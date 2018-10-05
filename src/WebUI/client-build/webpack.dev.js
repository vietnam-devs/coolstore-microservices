const commonPaths = require('./common-paths');

const webpack = require('webpack');
const CopyWebpackPlugin = require('copy-webpack-plugin');

const port = process.env.PORT || 5003;

const config = {
  mode: 'development',
  entry: {
    app: `${commonPaths.appEntry}/js/app.js`
  },
  output: {
    filename: '[name].[hash].js'
  },
  devtool: 'inline-source-map',
  module: {
    rules: []
  },
  plugins: [
    new CopyWebpackPlugin([
      {
        from: commonPaths.configPath + '/config.development.json',
        to: commonPaths.configPath + '/config.json'
      }
    ])
  ]
};

module.exports = config;
