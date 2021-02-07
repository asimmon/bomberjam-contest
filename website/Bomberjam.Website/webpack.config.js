const path = require('path');
const {CleanWebpackPlugin} = require('clean-webpack-plugin');

module.exports = (env, argv) => {
  return {
    mode: argv.mode === 'production' ? 'production' : 'development',
    entry: [
      path.resolve(__dirname, 'wwwroot/scripts/index.ts'),
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
          test: /\.ts$/,
          use: 'ts-loader',
          exclude: /node_modules/,
        },
        {
          test: /\.scss$/,
          use: ['style-loader', 'css-loader', 'postcss-loader', 'sass-loader']
        }
      ]
    },
    plugins: [
      new CleanWebpackPlugin()
    ]
  };
};