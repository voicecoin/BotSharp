import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button} from 'antd';
import { Nav, NavItem } from 'react-bootstrap';
import {TableView} from './Table';
import {Grid} from './Grid';
import {HTMLView} from './HTML';
const FormItem = Form.Item;
export class Tab extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : this.props.data,
      current : this.props.data[0].representType
    }
  }

  handleClick = (key) => {
    this.setState({
      current: key,
    });
  }

  render() {
    const navItem = this.state.data.length > 1 && this.state.data.map(values => <NavItem eventKey={values.representType} key={values.key}>{values.name}</NavItem>)

    return (
      <div style={{marginTop:'2%'}}>
        <Nav bsStyle="tabs" justified activeKey={this.state.current} onSelect={this.handleClick}>
          {navItem}
        </Nav>
        {
          this.state.data.length > 1 ?

            this.state.current == 1 ?
            <Grid data={this.state.data}/>
            :
            this.state.current == 2 ?
            <TableView data={this.state.data}/>
            :
            this.state.current == 3 && <HTMLView data={this.state.data}/>

          :

            this.state.data[0].representType == 1 ?
            <Grid data={this.state.data}/>
            :
            this.state.data[0].representType == 2 ?
            <TableView data={this.state.data}/>
            :
            this.state.data[0].representType == 3 && <HTMLView data={this.state.data}/>

        }

      </div>
    )
  }
}

const TabForm = Form.create()(Tab);
Tab.contextTypes = {
  router : React.PropTypes.object
}
export default TabForm
