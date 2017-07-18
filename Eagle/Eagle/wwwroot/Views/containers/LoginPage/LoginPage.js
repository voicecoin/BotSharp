import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select, Checkbox } from 'antd';
const FormItem = Form.Item;
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
const logoImg= require('../../Sources/images/o.png')
export class LoginContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      iconLoading:false
    }
  }

  handleSubmit = (e) => {
    e.preventDefault();
    this.setState({iconLoading:true});
    this.props.form.validateFields((err, values) => {
      if(err == null){
        let verification = {
          username : values.username,
          password : values.password
        };
        http.HttpAjax({
          url: DataURL + '/token',
          method:'POST',
          data:verification,
          headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
          }
        }).then((data) => {
          this.setState({iconLoading:false});
          localStorage.setItem('access_token', data.access_token);
          localStorage.setItem('user_id', data.user_id);
          document.cookie = 'access_token=' + data.access_token;
          document.cookie = 'user_id=' + data.user_id;
          this.context.router.push('Structure/Bundles');
        }).catch((err) => {
          console.log(err);
        });
      }
    })
  }


  render() {
    const { getFieldDecorator } = this.props.form;
    const loginStyle = {
      position: 'relative',
      width: '35%',
      marginLeft: '33%',
      marginTop: '10%'
    };
    return (
      <div style={loginStyle}>
        <img src={logoImg} width="50" className="logo" style={{width:'20%', height:'20%'}}/>
        <Form onSubmit={this.handleSubmit} className="login-form">
          <FormItem label='User Name' hasFeedback>
            {getFieldDecorator('username', {
              rules: [{ required: true, message: 'Please input your username!' }],
            })(
              <Input prefix={<Icon type="user" style={{ fontSize: 13 }} />} placeholder="Username" />
            )}
          </FormItem>
          <FormItem label='Password' hasFeedback>
            {getFieldDecorator('password', {
              rules: [{ required: true, message: 'Please input your Password!' }],
            })(
              <Input prefix={<Icon type="lock" style={{ fontSize: 13 }} />} type="password" placeholder="Password" />
            )}
          </FormItem>
          <FormItem>
            {getFieldDecorator('remember', {
              valuePropName: 'checked',
              initialValue: false,
            })(
              <Checkbox>Remember me</Checkbox>
            )}
            <Link><Button type="primary" className="login-form-button" size='large' style={{marginRight:'5%', marginLeft:'25%'}} onClick={this.handleSubmit}>Log in</Button></Link>
            OR<Link to='Register' style={{marginLeft:'5%'}}>register now!</Link>
            <Link to='ForgetPassword' style={{marginLeft:'5%'}}>forget password?</Link>
          </FormItem>
        </Form>
      </div>
    )
  }
}

const LoginContainerForm = Form.create()(LoginContainer);
LoginContainer.contextTypes = {
  router : React.PropTypes.object
}
export default LoginContainerForm
