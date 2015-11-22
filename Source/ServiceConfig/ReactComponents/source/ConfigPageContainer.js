var ReactDOM = require('react-dom'),
  React = require('react'),
  Reflux = require('reflux'),
  ConfigPage = require('./Components/ConfigPage'),
  Loading = require('./Components/Loading'),
  ErrorMessage = require('./Components/ErrorMessage'),
  ConfigStore = require('./Store/ConfigStore');

var ConfigPageContainer = React.createClass({
  mixins: [Reflux.connect(ConfigStore, "configStore")],
  render: function(){
    var store = this.state.configStore;
    if (!store) {
      return <Loading/>;
    }
    else if (store.status == 'error' && !store.config) {
      return <ErrorMessage/>;
    }
    else {
      return <ConfigPage store={this.state.configStore}/>;
    }
  }
});

ReactDOM.render(
  <ConfigPageContainer/>,
  document.getElementById('root')
);