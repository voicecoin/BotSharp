import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Modal, Select, Row, Col, Checkbox, Button, Card, Upload} from 'antd';
const FormItem = Form.Item;
import ImagesUploader from '../../components/ImagesUploader';
import Avatar from '../../components/Avatar';
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
const default_avatar = 'https://www.renderosity.com/templates/renderositybootstrap/image/default_avatar.png'
export class Settings extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : null,
      logo : default_avatar,
      visible : false
    }
  }
  fetchFn = () => {
          http.HttpAjax({
              url: 'test',
          }).then((data)=>{
            this.setState({data : data, logo : data.logo})
          }).catch((e)=>{
              console.log(e.message)
          })
  }
  componentDidMount() {
      // this.fetchFn();
  }
  beforeUpload = (file, fileList) => {
    this.readFile(file, data => {
      this.setState({logo : data});
    })
    return false;
  }
  readFile = (file, done) => {
    var reader = new FileReader();
    reader.onload = e => done(e.target.result);
    reader.readAsDataURL(file);
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
      this.setState({logo:newDataUrl});
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
    this.setState({logo:newavatar});
  }

  handleSubmit = () => {
    this.downScale(this.state.logo);
  }

  render() {
    const { getFieldDecorator } = this.props.form;
    const formStyle = {
      position: 'relative',
      marginTop: '5%',
      marginLeft: '10%'
    }
    const formItemLayout = {
      labelCol: { span: 6 },
      wrapperCol: { span: 8 },
    };
    const uploadLayout = {
      labelCol: { span: 6 },
      wrapperCol: { span: 2 },
    };
    const props = {
      name: 'file',
      showUploadList : false,
      beforeUpload : this.beforeUpload
    }
    return (
      <div style={{marginTop:'3%'}}>
        <div style={{position:'relative', marginTop: '5%', marginLeft:'4%'}}>
          <Avatar src={this.state.logo}/>
          {
            this.state.logo != default_avatar && <Button onClick={this.handleEdit} style={{marginLeft:'43%'}}>Edit</Button>
          }
        </div>
        <div style={{position:'relative', marginLeft: '43%', marginTop: '2%'}}>
          <Upload {...props}>
            <Button>
              <Icon type="upload" />Upload Logo
            </Button>
          </Upload>
        </div>
        <Form style={formStyle}>
          <FormItem
            label='Name'
            {...formItemLayout}
            hasFeedback
          >
            {getFieldDecorator('name', {
              rules: [{ required: true, message: 'Please input your Name!' }],
            })(
              <Input placeholder='Name'/>
            )}
          </FormItem>
          <FormItem
            {...formItemLayout}
            label='Description'
            hasFeedback
          >
            {getFieldDecorator('des', {
              rules: [{ required: true, message: 'Please input your Description!' }],
            })(
              <Input placeholder='Description'/>
            )}
          </FormItem>
        </Form>
        <div style={{position: 'relative', marginLeft: '50%'}}>
          <Button type="primary" style={{marginLeft:'2%'}} onClick={this.onConfirm}>Confirm</Button>
        </div>

        <Modal title='Edit Logo'
        visible={this.state.visible}
        onOk={this.handleOk}
        okText="Confirm"
        cancelText='Cancel'
        onCancel={this.handleCancel}
        >
        <div style={{marginLeft:'20%'}}>
          <ImagesUploader callbackParent={this.getImages} hasFile={true} display="none" width={300} height={300} src={this.state.logo}/>
        </div>
        </Modal>
      </div>
    )
  }
}

const SettingsForm = Form.create()(Settings);
Settings.contextTypes = {
  router : React.PropTypes.object
}
export default SettingsForm
