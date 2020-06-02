// Karma configuration file, see link for more information
// https://karma-runner.github.io/0.13/config/configuration-file.html

module.exports = function (config) {
  config.set({
    basePath: '',
    frameworks: ['jasmine', '@angular/cli'],
    plugins: [
      require('karma-jasmine'),
      require('@angular/cli/plugins/karma'),
      require('karma-junit-reporter'),
      require('karma-ie-launcher')
    ],
    client:{
      clearContext: false // leave Jasmine Spec Runner output visible in browser
    },
    files: [
      { pattern: './src/test.ts', watched: false }
    ],
    preprocessors: {
      './src/test.ts': ['@angular/cli']
    },
    mime: {
      'text/x-typescript': ['ts','tsx']
    },
    coverageIstanbulReporter: {
      reports: [ 'html', 'lcovonly' ],
      fixWebpackSourcePaths: true
    },
    angularCli: {
      environment: 'dev'
    },
    reporters: ['junit', 'progress'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    junitReporter: {
      outputDir: 'FrontendReports'
    },
    browsers: ['IE', 'IE10'],
    customLaunchers: {
      IE10: {
        base: 'IE',
        'x-ua-compatible': 'IE=EmulateIE10'
      }
    },
    singleRun: true,
    processKillTimeout: 5000
  });
};
