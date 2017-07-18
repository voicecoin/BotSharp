import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button} from 'antd';
const FormItem = Form.Item;
import BackButton from './GoBack';
import Http from './XmlHttp';
import {DataURL} from '../config/DataURL-Config';
const http = new Http();
export default class TopNav extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      profile :{}
    }
  }
  logout = () => {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_id');
    document.cookie = 'access_token=';
    document.cookie = 'user_id=';
    this.context.router.push('/');
  }

  getUserProfile = () => {
    http.HttpAjax({
      url: DataURL + '/api/Account',
      headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')}
    }).then((data) => {
      this.setState({profile:data})
    }).catch((err) => {
      console.log(err);
    });
  }

  componentDidMount() {
    this.getUserProfile();
  }



  render() {
    return (
      <div className='TopNav'>
        <div>
          <BackButton></BackButton>
          <Button type="omitted" className='goBack' style={{float:'right', marginRight:'10%'}} onClick={this.logout} icon='logout'>Log Out</Button>
          <div className='goBack' style={{display:'inline-block', color:'white', float:'right', marginRight:'2%', marginTop:'0.8%'}}>Welcome, {this.state.profile.userName}</div>
        </div>
      </div>
    )
  }
}

TopNav.contextTypes = {
  router : React.PropTypes.object
}
