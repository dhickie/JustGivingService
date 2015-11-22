var React = require('react'),
  InputField = require('./InputField'),
  SaveConfigButton = require('./SaveConfigButton'),
  ConfigPageActions = require('../ConfigPageActions'),
  EditStatus = require('./EditStatus');

var ConfigPage = React.createClass({
  render: function(){
    var store = this.props.store
    var config = store.config;
    var pageId = config.get('pageId');
    var pollingPeriod = config.get('pollingPeriod');
    var rainmeterExe = config.get('rainmeterExe');

    var onPageIdChange = function(event) {
      ConfigPageActions.onPageIdChange(event);
    };
    var onPollingPeriodChange = function(event) {
      ConfigPageActions.onPollingPeriodChange(event);
    };
    var onRainmeterExeChange = function(event) {
      ConfigPageActions.onRainmeterExeChange(event);
    }

    return (
      <div>
        <h1>Just Giving Service Configuration</h1>
        <InputField 
          inputDesc="Fundraising Page ID:" 
          placeholder="Page ID" 
          value={pageId}
          onChange={onPageIdChange}/>
        <InputField 
          inputDesc="Rainmeter exe location:" 
          placeholder="Drive path" 
          value={rainmeterExe}
          onChange={onRainmeterExeChange}/>
        <InputField 
          inputDesc="Polling period (ms):" 
          placeholder="Polling period" 
          value={pollingPeriod}
          onChange={onPollingPeriodChange}/>
        <SaveConfigButton/>
        <EditStatus status={store.status} errors={store.errors}/>
      </div>
    );
  }
});

module.exports = ConfigPage;