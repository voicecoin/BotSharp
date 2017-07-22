import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button} from 'antd';
import { Nav, NavItem } from 'react-bootstrap';
import {Tab} from './Tab';
import {TableView} from './Table';
import {Grid} from './Grid';
const FormItem = Form.Item;
import Http from '../XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();

export class View extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      tData : null
    }
  }

  fetchFn = () => {
    http.HttpAjax({
        url: DataURL + this.props.DataUrl,
        headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')}
    }).then((data)=>{
      this.setState({tData : data});
    }).catch((e)=>{
        console.log(e.message)
    })
  }
  componentDidMount() {
      this.fetchFn();
  }

  render() {
    const temp = this.state.tData && <Tab data={this.state.tData}/>

    return (
      <div>
        {temp}
      </div>
    )
  }
}

const ViewForm = Form.create()(View);
View.contextTypes = {
  router : React.PropTypes.object
}
export default ViewForm
