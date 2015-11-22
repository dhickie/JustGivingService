var React = require('react');

var ErrorMessage = React.createClass({
  render: function(){
    return (
      <div className="alert alert-danger" role="alert">
        <p>There was an error getting the current configuration from the JustGiving service.</p>
        <p>Try the following to correct the issue:</p>
        <ul>
          <li>Check the JustGiving service is running.</li>
          <li>Restart the JustGiving service.</li>
          <li>Restart the Tomcat service.</li>
          <li>Refresh this page.</li>
        </ul>
      </div>
    );
  }
});

module.exports = ErrorMessage;