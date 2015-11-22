var React = require('react');

var InputField = React.createClass({
  render: function(){
    var cmpnt = this;
    var inputDesc = cmpnt.props.inputDesc;
    var placeholder = cmpnt.props.placeholder;
    var value = cmpnt.props.value;
    var onChange = cmpnt.props.onChange;

    return (
      <p className="input-group">
        <span className="input-group-addon" id="pageId-addon">{inputDesc}</span>
        <input 
          type="text" 
          className="form-control"
          defaultValue={value}
          onChange={onChange}
          placeholder={placeholder} 
          aria-describedby="pageId-addon"/>
      </p>
    );
  }
});

module.exports = InputField;