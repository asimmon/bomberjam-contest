const path = require('path');
const {SourceMapDevToolPlugin} = require('webpack');
const {CleanWebpackPlugin} = require('clean-webpack-plugin');
const BrowserSyncPlugin = require('browser-sync-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = (env, argv) => {
  const isDevelopment = argv.mode !== 'production';
  const webpackPlugins = [];

  webpackPlugins.push(new CleanWebpackPlugin());
  webpackPlugins.push(new MiniCssExtractPlugin({ filename: 'bundle.css' }));

  if (isDevelopment) {
    webpackPlugins.push(new SourceMapDevToolPlugin({filename: '[file].map'}));
    webpackPlugins.push(new BrowserSyncPlugin({
      host: 'localhost',
      port: 5002,
      proxy: 'https://localhost:5001'
    }));
  }

  return {
    mode: isDevelopment ? 'development' : 'production',
    performance: {
      maxAssetSize: 1048576,
      maxEntrypointSize: 1048576
    },
    entry: [
      path.resolve(__dirname, 'Frontend/scripts/index.tsx'),
      path.resolve(__dirname, 'Frontend/styles/index.scss')
    ],
    output: {
      filename: 'bundle.js',
      path: path.resolve(__dirname, 'wwwroot/dist')
    },
    resolve: {
      extensions: ['.tsx', '.ts', '.js'],
    },
    module: {
      rules: [
        {
          test: /\.js$/,
          enforce: 'pre',
          use: isDevelopment ? ['source-map-loader'] : [],
        },
        {
          test: /\.tsx?$/,
          use: 'ts-loader',
          exclude: /node_modules/,
        },
        {
          test: /\.scss$/,
          use: [MiniCssExtractPlugin.loader, 'css-loader', 'postcss-loader', 'sass-loader']
        },
        {
          test: /\.(png|jpe?g|gif)$/i,
          use: [{ loader: 'url-loader' }]
        }
      ]
    },
    devtool: isDevelopment ? 'source-map' : false,
    plugins: webpackPlugins
  };
};