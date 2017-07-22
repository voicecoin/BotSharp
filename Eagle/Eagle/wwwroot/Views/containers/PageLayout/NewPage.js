import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select, Checkbox } from 'antd';
const FormItem = Form.Item;
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
const logoImg= require('../../Sources/images/logo.png')
export class NewPage extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
    }
  }

  handleSubmit = (e) => {
    e.preventDefault();
    this.props.form.validateFields((err, values) => {
      if(err == null){
        let data = {
          name : values.name,
          path : values.path
        };
        http.HttpAjax({
          url: DataURL + '/api/Page',
          method:'POST',
          data:data,
          headers: {
            'authorization':'Bearer ' + localStorage.getItem('access_token')
          }
        }).then((data) => {
          this.context.router.push('Structure/PageLayout');
        }).catch((err) => {
          console.log(err);
        });
      }
    })
  }

  handleSave = () => {

  }

  handleDelete = () => {
    // this.props.form.validateFields((err, values) => {
    //   if(err == null){
    //     let data = {
    //       name : values.name,
    //       path : values.path
    //     };
    //     http.HttpAjax({
    //       url: DataURL + '/api/Page',
    //       method:'DELETE',
    //       data:data,
    //       headers: {
    //         'authorization':'Bearer ' + localStorage.getItem('access_token')
    //       }
    //     }).then((data) => {
    //       this.context.router.push('Structure/PageLayout');
    //     }).catch((err) => {
    //       console.log(err);
    //     });
    //   }
    // })
  }


  render() {
    const { getFieldDecorator } = this.props.form;
    const Style = {
      position: 'relative',
      width: '35%',
      marginLeft: '33%',
      marginTop: '10%'
    };
    return (
      <div style={Style}>
        <Form onSubmit={this.handleSubmit} className="login-form">
          <FormItem label='Page Name' hasFeedback>
            {getFieldDecorator('name', {
              rules: [{ required: true, message: 'Please input your Page Name!' }],
            })(
              <Input placeholder="Page Name" />
            )}
          </FormItem>
          <FormItem label='Page Path' hasFeedback>
            {getFieldDecorator('path', {
              rules: [{ required: true, message: 'Please input your Page Path!' }],
            })(
              <Input placeholder="Page Path, i.e /Structure/Pages" />
            )}
          </FormItem>
        </Form>
        <div style={{marginLeft:'60%'}}>
          <Link><Button type="primary" className="login-form-button" size='large' style={{marginRight:'5%'}} onClick={this.handleDelete}>Delete</Button></Link>
          <Link><Button type="primary" className="login-form-button" size='large' style={{marginRight:'5%'}} onClick={this.handleSave}>Save</Button></Link>
          <Link><Button type="primary" className="login-form-button" size='large' style={{}} onClick={this.handleSubmit}>Create</Button></Link>
        </div>
      </div>
    )
  }
}

const NewPageForm = Form.create()(NewPage);
NewPage.contextTypes = {
  router : React.PropTypes.object
}
export default NewPageForm
