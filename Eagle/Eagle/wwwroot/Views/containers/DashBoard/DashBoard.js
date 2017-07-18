import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox,Button } from 'antd';
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
export default class DashBoardContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      tData : null
    }
  }

  fetchFn = () => {
    http.HttpAjax({
        url: DataURL,
    }).then((data)=>{
      this.setState({tData : data});
    }).catch((e)=>{
        console.log(e.message)
    })
  }
  componentWillMount() {
      // this.fetchFn();
  }

  render() {
    const landingStyle = {
      position: 'relative',
      marginTop:'25%',
      marginLeft:'40%'
    };
    return (
      <p>DashBoard</p>

    )
  }
}
