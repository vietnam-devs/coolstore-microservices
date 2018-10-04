const commonPaths = require('./common-paths');

const webpack = require('webpack');

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
  plugins: []
};

module.exports = config;
