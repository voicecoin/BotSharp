import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox,Button } from 'antd';
const logoImg = require('../../Sources/images/logo.png')
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
export default class LandingContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data: []
    }
  }

  fetchFn = () => {
          http.HttpAjax({
              url: DataURL + '/api/Registry/SiteSettings',
          }).then((data)=>{
            console.log(data);
            this.setState({data : data})
          }).catch((e)=>{
              console.log(e.message)
          })
  }
  componentDidMount() {
      this.fetchFn();
  }

  render() {
    const landingStyle = {
      position: 'relative',
      width: '35%',
      marginLeft: '33%',
      marginTop: '10%',
      textAlign:'center'
    };
    const buttonStyle = {
      marginTop:'5%',
      width:'150px',
      height:'30px'
    }
    return (
      <div style={landingStyle}>
            <div style={{ textAlign: 'center' }}>
                <img src={logoImg} width="50" className="logo" style={{ width: '20%', height: '20%' }} />
          <h2>{this.state.data.title}</h2>
          <p style={{position:'relative', top:'10px'}}>{this.state.data.slogan}</p>
        </div>
        <Link to='/Login'>
            <Button type='primary' style={buttonStyle}>Log In</Button>
        </Link>
      </div>
    )
  }
}
