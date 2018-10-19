const buildValidations = require('./client-build/build-validations');
const commonConfig = require('./client-build/webpack.common');
const webpackMerge = require('webpack-merge');

const addons = (/* string | string[] */ addonsArg) => {
  let addons = [...[addonsArg]] // Normalize array of addons (flatten)
    .filter(Boolean); // If addons is undefined, filter it out

  return addons.map(addonName => require(`./client-build/addons/webpack.${addonName}.js`));
};

module.exports = env => {
  if (!env) {
    throw new Error(buildValidations.ERR_NO_ENV_FLAG);
  }

  const envConfig = require(`./client-build/webpack.${env.env}.js`);
  const mergedConfig = webpackMerge(
    commonConfig,
    envConfig,
    ...addons(env.addons)
  );

  return mergedConfig;
};
