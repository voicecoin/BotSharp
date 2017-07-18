import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select, Modal} from 'antd';
import TreeView from 'treeview-react-bootstrap';
const FormItem = Form.Item;
const Option = Select.Option;

export class ViewsContainer extends React.Component {
  constructor(props){
    super(props);
    this.state = {

    }
  }
  render() {
    return (
      <p>test</p>
    )
  }
}
const ViewForm = Form.create()(ViewsContainer);
ViewsContainer.contextTypes = {
  router: React.PropTypes.object
}
export default ViewForm
