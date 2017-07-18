import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button } from 'antd';
const FormItem = Form.Item;
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
import {View} from '../../components/View/View'
export class SharedContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      tData : []
    }
  }

  fetchFn = () => {
    let query = '';
    for(var i in this.props.location.query){
      query += this.props.location.query[i];
    }
    http.HttpAjax({
        url: DataURL + query,
    }).then((data)=>{
      this.setState({tData : data});
    }).catch((e)=>{
        console.log(e.message)
    })
  }
  componentWillMount() {
      this.fetchFn();
  }
  render() {
    return (
      this.state.tData.length != 0 &&
      <View data={this.state.tData} />
    )
  }
}

const SharedContainerForm = Form.create()(SharedContainer);
SharedContainer.contextTypes = {
  router : React.PropTypes.object
}
export default SharedContainerForm
