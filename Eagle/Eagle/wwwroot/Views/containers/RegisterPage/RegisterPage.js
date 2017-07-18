import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button } from 'antd';
const FormItem = Form.Item;
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
const RegisterLogo = require('../../Sources/images/o.png');
import Recaptcha from 'react-grecaptcha';
export class RegisterContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
    }
  }

  handleSubmit = (e) => {
    e.preventDefault();
    this.setState({
        iconLoading:true
    });
    this.props.form.validateFields((err, values) => {
      if(err == null){
        var newuser = {
          'UserName' : values.email,
          'Password' : values.password
        }
        http.HttpAjax({
          url: DataURL + '/api/Account',
          method:'POST',
          data:newuser,
          headers: {
            'Content-Type': 'application/json'
          }
        }).then((data) => {
          this.context.router.push('/Login');
        }).catch((err) => {
          console.log(err);
        });
      }
    });

  }

  checkPassword = (rule, value, callback) => {
    const form = this.props.form;
    if (value && value !== form.getFieldValue('password')) {
      callback('Two passwords that you enter is inconsistent!');
    } else {
      callback();
    }
  }

  checkConfirm = (rule, value, callback) => {
    const form = this.props.form;
    if (value && this.state.confirmDirty) {
      form.validateFields(['confirm'], { force: true });
    }
    callback();
  }

  verifyCallback = (response) => {
    console.log(response);
    this.props.form.setFields({verification:{
      value : 'YES'
    }});
  }
  expiredCallback = () => {
    console.log('expired');
    this.props.form.setFields({verification:{
      errors: [new Error('Error Occured.')]
    }});
  }


  render() {
    const { getFieldDecorator } = this.props.form;

    const registerStyle = {
      position: 'relative',
      width: '30%',
      marginLeft: '35%',
      marginTop:'10%'
    };
    const tailFormItemLayout = {
      wrapperCol: {
        xs: {
          span: 24,
          offset: 0,
        },
        sm: {
          span: 14,
          offset: 6,
        },
      },
    };
    return (
      <div style={registerStyle}>
        <img src={RegisterLogo} width="50" className="logo" style={{width:'20%', height:'20%'}}/>
        <Form style={{marginTop:'10%'}} className="login-form">
          <FormItem
            label="E-mail"
            hasFeedback
          >
            {getFieldDecorator('email', {
              rules: [{
                type: 'email', message: 'The input is not valid E-mail!',
              }, {
                required: true, message: 'Please input your E-mail!',
              }],
            })(
              <Input prefix={<Icon type="mail" style={{ fontSize: 13 }} />} placeholder="Input your E-mail" />
            )}
          </FormItem>
          <FormItem
            label="Password"
            hasFeedback
          >
            {getFieldDecorator('password', {
              rules: [{
                required: true, message: 'Please input your password!',
              }, {
                validator: this.checkConfirm,
              }],
            })(
              <Input type="password" prefix={<Icon type="lock" style={{ fontSize: 13 }}/>}  placeholder="Input your Password"/>
            )}
          </FormItem>
          <FormItem
            label="Confirm Password"
            hasFeedback
          >
            {getFieldDecorator('confirm', {
              rules: [{
                required: true, message: 'Please confirm your password!',
              }, {
                validator: this.checkPassword,
              }],
            })(
              <Input prefix={<Icon type="lock" style={{ fontSize: 13 }} />} type="password" onBlur={this.handleConfirmBlur} placeholder="Re-Input your Password" />
            )}
          </FormItem>
          <FormItem
            label="verification"
            hasFeedback
          >
            {getFieldDecorator('verification', {
              rules: [{
                required: true, message: 'Please confirm your verification!',
              }],
            })(
                <Recaptcha
                  sitekey="6LczxSgUAAAAAJv9QJvzAVn5AMZdMTSdZjednOFB"
                  callback={this.verifyCallback}
                  expiredCallback={this.expiredCallback}
                  locale="en-US"
                  className="customClassName"
                  style={{width:'400'}}
              />
            )}
          </FormItem>
          <FormItem {...tailFormItemLayout}>
            <Button type="primary" size="large" onClick={this.handleSubmit} style={{position:'relative', marginLeft:'30%'}}>Register</Button>
            <Link to='Login' style={{float:'right'}}>Back to Login</Link>
          </FormItem>

        </Form>
      </div>
    )
  }
}

const RegisterContainerForm = Form.create()(RegisterContainer);
RegisterContainer.contextTypes = {
  router : React.PropTypes.object
}
export default RegisterContainerForm
