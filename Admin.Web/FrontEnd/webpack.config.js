const path = require('path');
const nodeExternals = require('webpack-node-externals');

var config = [
    //web configuration
    {
        entry: ['./src/main.tsx'],
        output: {
            path: path.resolve(__dirname, './public/build'),
            filename: 'bundle.js',
        },
        resolve: {
            //automatically infer '.ts' and '.tsx' when importing files
            extensions: ['.js', '.jsx', '.ts', '.tsx']
        },
        module: {
            rules: [
                {
                    test:/\.css$/,
                    use:['style-loader','css-loader'],
                },
                {
                    test:/\.tsx?$/,
                    include: path.resolve(__dirname, "./src/"),
                    loader: "awesome-typescript-loader"
                }
            ]
        },
        devtool: "source-map"
    }
];
module.exports = config;