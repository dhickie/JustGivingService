var React = require('react');

var EditStatus = React.createClass({
  render: function(){
    var status = this.props.status;
    var errors = this.props.errors;

    var statusText = null;
    if (status == "saved") {
      statusText = (
        <div className= "alert alert-success" role="alert">
          This configuration is saved.
        </div>
      );
    }
    else if (status == "modified") {
      statusText = (
        <div className= "alert alert-warning" role="alert">
          This configuration has unsaved changes.
        </div>
      );
    }
    else if (status == "error") {
      // If there are errors then put them in to a bullet list
      var errorBullets = errors.map(function(error, idx){
        return (
          <li key={idx}>{error}</li>
        );
      });
      statusText = (
        <div className="alert alert-danger" role="alert">
          This configuration has errors:
          <ul>
            {errorBullets}
          </ul>
        </div>
      );
    }

    return statusText;
  }
});

module.exports = EditStatus;