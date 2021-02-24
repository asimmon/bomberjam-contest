const path = require('path');
const {SourceMapDevToolPlugin} = require("webpack");
const {CleanWebpackPlugin} = require('clean-webpack-plugin');
const BrowserSyncPlugin = require('browser-sync-webpack-plugin');

module.exports = (env, argv) => {
  return {
    mode: argv.mode === 'production' ? 'production' : 'development',
    entry: [
      path.resolve(__dirname, 'wwwroot/scripts/index.tsx'),
      path.resolve(__dirname, 'wwwroot/styles/index.scss')
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
          enforce: "pre",
          use: ["source-map-loader"],
        },
        {
          test: /\.tsx?$/,
          use: 'ts-loader',
          exclude: /node_modules/,
        },
        {
          test: /\.scss$/,
          use: ['style-loader', 'css-loader', 'postcss-loader', 'sass-loader']
        }
      ]
    },
    devtool: "source-map",
    plugins: [
      new CleanWebpackPlugin(),
      new SourceMapDevToolPlugin({
        filename: "[file].map"
      }),
      new BrowserSyncPlugin({
        host: 'localhost',
        port: 5002,
        proxy: 'https://localhost:5001',
      }, {
        reload: true
      })
    ]
  };
};