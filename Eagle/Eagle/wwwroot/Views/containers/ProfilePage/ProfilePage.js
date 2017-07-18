import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button, Upload, Modal } from 'antd';
const FormItem = Form.Item;
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
import Avatar from '../../components/Avatar';
import ImagesUploader from '../../components/ImagesUploader';
import { Nav, NavItem } from 'react-bootstrap';
const http = new Http();
const default_avatar = 'https://www.renderosity.com/templates/renderositybootstrap/image/default_avatar.png'
export class ProfileContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      current: 'profile',
      loading : true,
      tData : {
        firstName : null,
        lastName : null,
        email : null,
        avatar : null
      },
      visible:false,
      avatar : default_avatar,
      query : null
    }
    console.log(props);
  }

  fetchFn = () => {
    let query = '';
    for(var i in this.props.location.query){
      query += this.props.location.query[i];
    }
    this.setState({query : query});
    http.HttpAjax({
        url: DataURL + '/api/Account/' + query
    }).then((data)=>{
      if(data.avatar != null) this.setState({avatar : data.avatar});
      if(data.email != null || data.firstName != null || data.lastName != null){
        this.setState({tData:data,loading:false});
      }
    }).catch((e)=>{
        console.log(e.message)
    })
  }
  componentDidMount() {
      this.fetchFn();
  }
  onConfirm = () => {
    this.props.form.validateFields((err, values) => {
      if(err == null){
        this.downScale(this.state.avatar);
        let profile = {
          id : localStorage.user_id,
          firstName : values.firstName,
          lastName : values.lastName,
          email : values.email,
          avatar : this.state.avatar
        }
        this.setState({
          tData : profile
        });
        http.HttpAjax({
          url: DataURL + '/api/UserEntity/' + this.state.query,
          method:'PUT',
          data: profile,
          headers: {
            'Content-Type': 'application/json',
            'authorization':'Bearer ' + localStorage.getItem('access_token')
          },
        }).then((data) => {
          console.log('success');
        }).catch((err) => {
          console.log(err);
        })
      }
    });
    this.props.form.resetFields();
  }
  beforeUpload = (file, fileList) => {
    this.readFile(file, data => {
      this.setState({avatar:data});
    })
    return false;
  }
  readFile = (file, done) => {
    var reader = new FileReader();
    reader.onload = e => done(e.target.result);
    reader.readAsDataURL(file);
  }
  handleClick = (key) => {
    this.setState({
      current: key,
    });
  }
  downScale = (dataUrl) => {
    var imageType = "image/jpeg";
    var imageArguments = 0.7;
    var newHeight = 128;
    var newWidth = 128;
    var image = new Image();
    image.src = dataUrl;
    var canvas = document.createElement("canvas");
    canvas.width = newWidth;
    canvas.height = newHeight;
    var ctx = canvas.getContext("2d");
    var newDataUrl = ''
    image.onload = () => {
      ctx.drawImage(image, 0, 0, newWidth, newHeight);
      newDataUrl = canvas.toDataURL(imageType, imageArguments);
      this.setState({avatar:newDataUrl});
    }
  }
  handleOk = () => {
    this.setState({visible:false});
  }
  handleCancel =() => {
    this.setState({visible:false});
  }
  handleEdit = () => {
    this.setState({visible:true});
  }
  getImages = (img) => {
    let newavatar = img.tempimg;
    this.setState({avatar:newavatar});
  }

  render() {
    const divStyle = {
      position: 'relative',
      marginTop: '10%',
      marginLeft: '45%'
    };
    const formItemLayout = {
      labelCol: { span: 6 },
      wrapperCol: { span: 8 },
    };
    const formStyle = {
      position: 'relative',
      marginTop: '5%',
      marginLeft: '10%'
    }
    const props = {
      name: 'file',
      showUploadList : false,
      beforeUpload : this.beforeUpload
    }
    const { getFieldDecorator } = this.props.form;
    return (
      <div>
      <div style={{marginTop:'3%'}}>
        <Nav bsStyle="tabs" justified activeKey={this.state.current} onSelect={this.handleClick}>
          <NavItem eventKey='profile'>User Profile</NavItem>
          <NavItem eventKey='test'>NavItem 2 content</NavItem>
        </Nav>
      </div>
        {
        this.state.current == 'profile' ?
        <div style={{position:'relative', marginTop: '5%'}}>
          <div>
            <Avatar src={this.state.avatar}/>
            {
              this.state.avatar != default_avatar && <Button onClick={this.handleEdit} style={{marginLeft:'43%'}}>Edit</Button>
            }
          </div>
          <div style={{position:'relative', marginLeft: '41%', marginTop: '2%'}}>
            <Upload {...props}>
              <Button>
                <Icon type="upload" />Upload Avatar
              </Button>
            </Upload>

          </div>
          <div>
            <Form style={formStyle}>
              <FormItem
                label='First Name'
                {...formItemLayout}
                hasFeedback
              >
                {getFieldDecorator('firstName', {
                  rules: [{ required: true, message: 'Please input your First Name!' }],
                  initialValue:this.state.tData.firstName
                })(
                  <Input placeholder='First Name'/>
                )}
              </FormItem>
              <FormItem
                {...formItemLayout}
                label='Last Name'
                hasFeedback
              >
                {getFieldDecorator('lastName', {
                  rules: [{ required: true, message: 'Please input your Last Name!' }],
                  initialValue:this.state.tData.lastName
                })(
                  <Input placeholder='Last Name'/>
                )}
              </FormItem>
              <FormItem
                {...formItemLayout}
                label='E-mail'
                hasFeedback
              >
                {getFieldDecorator('email', {
                  rules: [
                    { required: true, message: 'Please input your Last Name!' },
                    {
                      type: 'email', message: 'The input is not valid E-mail!',
                    }
                  ],
                  initialValue:this.state.tData.email
                })(
                  <Input type='email' placeholder='E-mail'/>
                )}
              </FormItem>
            </Form>
            <div style={{position: 'relative', marginLeft: '50%'}}>
              <Button type="primary" style={{marginLeft:'2%'}} onClick={this.onConfirm}>Confirm</Button>
            </div>
          </div>
        </div>
        :
        <p>test</p>
        }
        <Modal title='Edit Avatar'
        visible={this.state.visible}
        onOk={this.handleOk}
        okText="Confirm"
        cancelText='Cancel'
        onCancel={this.handleCancel}
        >
        <div style={{marginLeft:'20%'}}>
          <ImagesUploader callbackParent={this.getImages} hasFile={true} display="none" width={300} height={300} src={this.state.avatar}/>
        </div>
        </Modal>
      </div>
    )
  }
}



const ProfileContainerForm = Form.create()(ProfileContainer);
ProfileContainer.contextTypes = {
  router : React.PropTypes.object
}
export default ProfileContainerForm
