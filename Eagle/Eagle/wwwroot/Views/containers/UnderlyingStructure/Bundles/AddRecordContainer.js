import React, {Component} from 'react';
import {Button,Input, Row, Col,Radio,InputNumber,Form, Upload, Icon} from 'antd'
import  {getUrlParams} from '../../../components/Utils.js';
import NoData from '../../../components/NoData';
import {DataURL} from '../../../config/DataURL-Config'
//For google map and google autocomplete components
import GooglePlacesAutocomplete from '../../../components/GooglePlacesAutocomplete';
import GoogleMap from '../../../components/GoogleMap';
//End google components
//For Image Upload components
import ImagesUploader from '../../../components/ImagesUploader';
//End ImageUpload component
//For rich editor
import ReactQuill from 'react-quill'
//End rich editor
//Http server
import Http from '../../../components/XmlHttp';
const http = new Http();
//End Http server
const RadioGroup = Radio.Group;
const FormItem = Form.Item;
const NoDataImg = require('../../../Sources/images/no-one.png');
export  class Entities extends React.Component{
        constructor(props){
            super(props)
            this.state={
                tData:[],
                value:1,
                iconLoading:true,
                address : '',
                initialLat : 41.8987699,
                initialLng : -87.62291679999998,
                lat : null,
                lng : null,
                bundleId : null,
                hasFile : false,
                fileSource : [],
                CanvasWidth : 0,
                CanvasHeight : 0,
                crop : false,
                tempimg : null,
                State : null,
                City : null,
                richValue : ''
            }
            this.geturl(getUrlParams(window.location.href).bundleId);
            this.GooglePlacesOnChange = this.GooglePlacesOnChange.bind(this);
        }
        geturl(newbundleId){
            this.getRecordAddfetchFn(newbundleId);
        }

        getRecordAddfetchFn = (newbundleId) => {
            http.HttpAjax({
                url: DataURL + '/api/Bundle/' + newbundleId
            }).then((data)=>{
                  this.setState({tData:data.fields, bundleId : newbundleId, iconLoading:false})
            }).catch((e)=>{
                console.log(e.message)
            })
        }

        gotoRecordAdd=()=>{
            let path = `/Structure/Bundles`
            this.context.router.push(path)
        }

        onRichEditorChange = (value) => {
          this.setState({richValue:value});
        }

        handleSubmit= (e) => {
            e.preventDefault();
            this.setState({
                iconLoading:true
            });
            this.props.form.validateFields((err,values)=>{
                if(!err){
                    this.setState({
                        iconLoading:false
                    });
                    values.Address = this.state.address;
                    if(this.state.fileSource.length > 0) values.Photo = this.state.fileSource;
                    const tdata = this.state.tData;
                    var tempbundleId = this.state.bundleId;
                    var newvalue = {
                      'bundleId' : this.state.bundleId,
                      'fieldRecords' : []
                    };
                    if(values.Name){
                      newvalue.name = values.Name;
                    }
                    else newvalue.name = '';
                    if(values.Description){
                      newvalue.description = values.Description;
                    }
                    else newvalue.description = '';
                    for(var i = 0; i < tdata.length; i ++){
                      var record = {};
                      record.data = [];
                      record.fieldTypeId = tdata[i].fieldTypeId;
                      record.bundleFieldId = tdata[i].id;
                      if(values[tdata[i].name] && tdata[i].fieldTypeId != 4 && tdata[i].fieldTypeId != 6) {
                        var temp = {};
                        temp.value = values[tdata[i].name];
                        record.data.push(temp);
                        newvalue.fieldRecords.push(record);
                      }
                      else if(values[tdata[i].name] && tdata[i].fieldTypeId == 6){
                        var temp = {
                          'City' : 'Chicago',
                          'State' : 'IL'
                        }
                        record.data.push(temp);
                        newvalue.fieldRecords.push(record);
                      }
                      else if(values[tdata[i].name] && tdata[i].fieldTypeId == 4){
                        record.data = this.state.fileSource;
                        newvalue.fieldRecords.push(record);
                      }
                    }

                    if(this.state.richValue.length != 0){
                      newvalue.fieldRecords.push({
                        fieldTypeId:7,
                        data : [{
                          value: this.state.richValue.toString('html')
                        }]
                      });
                    }
                    console.log(newvalue);
                    http.HttpAjax({
                      url: DataURL + '/api/Node',
                      method:'POST',
                      data:newvalue,
                      headers: {
                        'Content-Type': 'application/json',
                        'authorization':'Bearer ' + localStorage.access_token
                      },
                    }).then((data) => {
                      this.setState({iconLoading:false});
                    }).catch((err) => {
                      console.log(err);
                    });
                    //POST Method for submit data, do it later

                }else{
                    this.setState({
                        iconLoading:false
                    })
                }
            })
        }

          GooglePlacesOnChange(newstate){
              this.setState({
                address : newstate.address,
                lat : newstate.lat,
                lng : newstate.lng,
                initialLat : newstate.lat,
                initialLng : newstate.lng
              })
          }

          getImages = (img) => {
            this.setState({
              hasFile: img.hasFile,
              tempimg : img.tempimg,
              CanvasWidth: img.CanvasWidth,
              CanvasHeight : img.CanvasHeight,
              crop : img.crop,
              fileSource : img.fileSource
            });
          }
          render(){
              const  {getFieldDecorator}  = this.props.form;
              const formItemLayout = {
                  labelCol: { span: 6 },
                  wrapperCol: { span: 8 },
              };
              const richLayout = {
                  labelCol: { span: 6 },
                  wrapperCol: { span: 18 },
              };
              const formStyle = {
                  display:'inline-block',
                  width:'70%',
                  position:'relative'
              }
              const attribute={
                  text:'No DATA',
                  imgSrc:NoDataImg
              }
              const modules = {
                toolbar : [
                            ['bold', 'italic', 'underline', 'strike'],
                            ['blockquote', 'code-block'],
                            [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                            [{ 'indent': '-1'}, { 'indent': '+1' }],
                            [{ 'direction': 'rtl' }],
                            [{ 'size': ['small', false, 'large', 'huge'] }],
                            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                            [{ 'color': [] }, { 'background': [] }],
                            [{ 'font': [] }],
                            [{ 'align': [] }],
                            ['clean']
                          ]
              }
              const inputs = this.state.tData.map((value,index)=>{
                    switch(value.fieldTypeId){
                      //currently manually define the label of input
                        case FieldsType.Address:
                              return  <FormItem key={index}
                                          {...formItemLayout}
                                          label='Address'
                                          hasFeedback
                                      >
                                          {getFieldDecorator('Address',{
                                              rules:[
                                                  {required:false,message:'Please enter the Address forma'}
                                              ]
                                          })(
                                            <div className='AddressOrRecord'>
                                              <GooglePlacesAutocomplete callbackParent={this.GooglePlacesOnChange} />
                                              <GoogleMap initialLat={this.state.initialLat} initialLng={this.state.initialLng} lat={this.state.lat} lng={this.state.lng} address={this.state.address} ></GoogleMap>
                                            </div>
                                          )}
                                      </FormItem>

                        case FieldsType.Boolean:
                              return  <FormItem key={index}
                                          {...formItemLayout}
                                          label={value.name}
                                          hasFeedback
                                      >
                                      {getFieldDecorator(value.name,{
                                          rules:[
                                              {required:false,message:'Please enter the Currency format'}
                                          ]
                                      })(
                                          <RadioGroup>
                                              <Radio value="yes">Yes</Radio>
                                              <Radio value="no">No</Radio>
                                              </RadioGroup>
                                      )}
                                      </FormItem>
                        case FieldsType.Currency:
                             return   <FormItem key={index}
                                         {...formItemLayout}
                                         label={value.name}
                                         hasFeedback
                                     >
                                         {getFieldDecorator(value.name,{
                                             rules:[
                                                 {required:false,message:'Please enter the Currency format'}
                                             ]
                                         })(
                                             <Input type='Text' placeholder={value.name}/>
                                         )}
                                     </FormItem>




                        case FieldsType.Email:
                              return  <FormItem  key={index}
                                          {...formItemLayout}
                                          label={value.name}
                                          hasFeedback
                                      >
                                          {getFieldDecorator(value.name,{
                                              rules:[
                                                  {required: false, message: 'Please enter the Email format'},
                                                  {type : 'email', message : 'The input is not valid Email'}
                                              ]
                                          })(
                                              <Input type='Email' placeholder={value.name}/>
                                          )}
                                      </FormItem>
                        case FieldsType.Number:
                              return  <FormItem key={index}
                                          {...formItemLayout}
                                          label={value.name}
                                          hasFeedback
                                      >
                                          {getFieldDecorator(value.name,{
                                              rules:[
                                                  {required:false,message:'Please enter the Number forma'}
                                              ]
                                          })(
                                              <InputNumber  min={1} max={1000} defaultValue={3}  placeholder={value.name}  />
                                          )}
                                      </FormItem>
                        case FieldsType.Text:
                              return  <FormItem  key={index}
                                          {...formItemLayout}
                                          label={value.name}
                                          hasFeedback
                                      >
                                          {getFieldDecorator(value.name,{
                                              rules:[
                                                  {required:false,message:'Please enter the String forma'}
                                              ]
                                          })(
                                              <Input placeholder={value.name}/>
                                          )}
                                      </FormItem>



                        case FieldsType.Image:
                              return  <FormItem  key={index}
                                          {...formItemLayout}
                                          label='Photo'
                                          hasFeedback
                                      >
                                          {getFieldDecorator('Photo',{
                                              rules:[
                                                  {required:false,message:'Please Upload Photo'}
                                              ]
                                          })(
                                            <ImagesUploader callbackParent={this.getImages} canUpload={true} hasFile={false}/>
                                          )}
                                      </FormItem>

                        case FieldsType.RichText:
                              return (

                                <FormItem key={index}
                                          {...richLayout}
                                          label={value.name}
                                          hasFeedback
                                >
                                  <ReactQuill value={this.state.richValue}
                                              onChange={this.onRichEditorChange}
                                              placeholder='Input Your Text'
                                              modules={modules}
                                              >
                                              <div style={{height:'150px'}}/>
                                  </ReactQuill>
                                </FormItem>

                              )
                    }
              })


              return(
                  <div className='table' style={{marginTop:'4%'}}>
                          <Form onSubmit={this.handleSubmit} style={formStyle}>
                              <FormItem  key='name'
                                  {...formItemLayout}
                                  label="Name"
                                  hasFeedback
                              >
                                  {getFieldDecorator("Name",{
                                      rules:[
                                          {required:false,message:'Please enter the String forma'}
                                      ]
                                  })(
                                      <Input placeholder="Name"/>
                                  )}
                              </FormItem>
                              <FormItem  key='des'
                                  {...formItemLayout}
                                  label="Description"
                                  hasFeedback
                              >
                                  {getFieldDecorator("Description",{
                                      rules:[
                                          {required:false,message:'Please enter the String forma'}
                                      ]
                                  })(
                                      <Input type='textarea' rows={4} placeholder="Description"/>
                                  )}
                              </FormItem>
                              {inputs}
                              <FormItem
                                  wrapperCol={{ span: 2, offset: 5 }}
                              >
                              <Button  type="primary" htmlType="submit" loading={this.state.iconLoading} style={{marginLeft:'50%'}}>Submit</Button>
                              </FormItem>
                          </Form>
                  </div>

              )
          }
}
Entities.contextTypes = {
    router: React.PropTypes.object
}

let FieldsType;
    (function (FieldsType) {
         FieldsType[FieldsType["Text"] = 1] = "Text";
         FieldsType[FieldsType["Number"] = 2] = "Number";
         FieldsType[FieldsType["Currency"] = 3] = "Currency";
         //Added 'Image' FieldsType
         FieldsType[FieldsType["Image"] = 4] = "Image";
         FieldsType[FieldsType["Boolean"] = 5] = "Boolean";
         //added 'Address' FieldsType
         FieldsType[FieldsType["Address"] = 6] = "Address";
         //added 'richtext' FieldsType
         FieldsType[FieldsType["RichText"] = 7] = "RichText";

})(FieldsType = Entities.FieldsType || (Entities.FieldsType = {}));
const EntitiesFrom = Form.create()(Entities);
export default EntitiesFrom
