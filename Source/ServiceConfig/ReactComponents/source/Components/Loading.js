var React = require('react');

var Loading = React.createClass({
  render: function(){
    return (
      <img src="loading.gif" alt="Loading..." style={{width:200,height:200}}/>
    );
  }
});

module.exports = Loading;