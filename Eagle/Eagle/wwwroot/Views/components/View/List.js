import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button, Timeline } from 'antd';
const FormItem = Form.Item;
export class List extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : this.props.data
    }
  }

  render() {
    const data = this.state.data[0].data.map((values) =>
    <Timeline key={values.id}>
      <Timeline.Item>{values.userName}</Timeline.Item>
      <Timeline.Item>{values.lastName}</Timeline.Item>
      <Timeline.Item>{values.firstName}</Timeline.Item>
      <Timeline.Item>{values.rowVersion}</Timeline.Item>
      <Timeline.Item>{values.bundleId}</Timeline.Item>
      <Timeline.Item>{values.email}</Timeline.Item>
    </Timeline>
    )
    return (
      <div style={{marginTop:'3%', marginLeft:'30%'}}>
          {data}
      </div>
    )
  }
}

const ListForm = Form.create()(List);
List.contextTypes = {
  router : React.PropTypes.object
}
export default ListForm
