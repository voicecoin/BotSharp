import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button} from 'antd';
import { Nav, NavItem } from 'react-bootstrap';
import {Tab} from './Tab';
import {TableView} from './Table';
import {Grid} from './Grid';
const FormItem = Form.Item;

export class View extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : this.props.data
    }
  }


  render() {
    return (
      <Tab data={this.props.data}/>
    )
  }
}

const ViewForm = Form.create()(View);
View.contextTypes = {
  router : React.PropTypes.object
}
export default ViewForm
