var React = require('react'),
  ConfigPageActions = require('../ConfigPageActions');

var SaveConfigButton = React.createClass({
  render: function(){
    return (
      <button 
        type="button" 
        className="btn btn-default" 
        onClick={ConfigPageActions.onSaveConfig}>
        Save
      </button>
    );
  }
});

module.exports = SaveConfigButton;