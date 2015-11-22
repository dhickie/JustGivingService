var Reflux = require('reflux');

var ConfigPageActions = Reflux.createActions([
  'onPageIdChange',
  'onPollingPeriodChange',
  'onRainmeterExeChange',
  'onSaveConfig'
]);

module.exports = ConfigPageActions;