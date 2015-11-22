var Reflux = require('reflux'),
  Immutable = require('Immutable'),
  ConfigPageActions = require('../ConfigPageActions');

var ConfigStore = Reflux.createStore({
  listenables: ConfigPageActions,

  // Initialisation
  getInitialState: function(){
    var store = this;
    store.state = {};

    $.ajax({
      url: './servlet',
      dataType: 'json',
      success: function(data){
        store.state.config = Immutable.fromJS(data);
        store.state.status = 'saved';
        store.refreshState();
        setTimeout(store.refreshState, 1000);
      },
      error: function(request, textStatus, errorThrown){
        store.state.status = 'error';
        store.state.config = null;
        store.refreshState();
      }
    });
  },
  refreshState: function(){
    this.trigger(this.state);
  },
  setStatus: function(){
    if (this.state.status != 'error') {
      this.state.status = 'modified';
    }
  },

  // Action handlers
  onPageIdChange: function(event){
    var newValue = event.target.value;

    config = this.state.config;
    config = config.set('pageId', newValue);
    this.state.config = config;

    this.setStatus();
    this.refreshState();
  },
  onPollingPeriodChange: function(event){
    var newValue = event.target.value;

    config = this.state.config;
    config = config.set('pollingPeriod', newValue);
    this.state.config = config;

    this.setStatus();
    this.refreshState();
  },
  onRainmeterExeChange: function(event){
    var newValue = event.target.value;

    config = this.state.config;
    config = config.set('rainmeterExe', newValue);
    this.state.config = config;

    this.setStatus();
    this.refreshState();
  },
  onSaveConfig: function(){
    // Convert the current configuration back in to a json string
    var store = this;
    var jsonObject = store.state.config.toJSON();
    var jsonString = JSON.stringify(jsonObject);

    // Send the json off to the configuration servlet
    $.ajax({
      url: './servlet',
      type: 'post',
      dataType: 'json',
      data: jsonString,
      success: function(data) {
        // Check whether some validation errors have been returned
        if (data.errors) {
          store.state.errors = Immutable.fromJS(data.errors);
          store.state.status = 'error';
          store.refreshState();
        }
        else {
          store.state.config = Immutable.fromJS(data);
          store.state.status = 'saved';
          store.refreshState();
        }
      },
      error: function(request, textStatus, errorThrown) {
        // If there was an error in the servlet, display an error
        store.state.errors = Immutable.fromJS([
          'An unknown error occured when saving the configuration. Refreshing the page and trying again may fix the issue.'
        ]);
        store.state.status = 'error';
        store.refreshState();
      }
    });
  }
});

module.exports = ConfigStore;