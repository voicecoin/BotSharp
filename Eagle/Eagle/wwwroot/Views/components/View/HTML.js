import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button, Card} from 'antd';
const FormItem = Form.Item;
export class HTMLView extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : this.props.data
    }
  }

  render() {

    return (
      <div style={{marginTop:'3%', textAlign:'center'}}>
        HTML
      </div>
    )
  }
}

const HTMLViewForm = Form.create()(HTMLView);
HTMLView.contextTypes = {
  router : React.PropTypes.object
}
export default HTMLViewForm
