  import {
    Form, Select, InputNumber, Switch, Radio,
    Slider, Button, Upload, Icon,Input
  } from 'antd';

  import React from 'react';
  import  {getUrlParams} from '../../../components/Utils.js';
  import {DataURL} from '../../../config/DataURL-Config'
  import Http from '../../../components/XmlHttp'
  const http = new Http();
  const FormItem = Form.Item;
  const Option = Select.Option;
  const bundleId =''
  export class NewFields extends React.Component{
        constructor(props){
          super(props)
          this.state={
            optionValue:[],
            iconLoading:false
          }
          let wurl = window.location.href
          let newbundleId = getUrlParams(wurl).bundleId;
          this.getBundleId(newbundleId);
          this.fetchOptionValue();
        }

        getBundleId(newbundleId){
          this.bundleId=newbundleId;
        }

        handleSubmit = (e) =>{
          e.preventDefault();
          this.setState({
            iconLoading:true
          })
          this.props.form.validateFields((err,values)=>{
              if(!err){
                 this.sendNewFields(values,this.bundleId)

              }
          });
        }

        fetchOptionValue = (bundleId) => {
            http.HttpAjax({
              url: DataURL + '/api/Field/Types'
            }).then((data)=>{
               this.setState({optionValue:data})
            }).catch((e) => { console.log(e.message) })
        }

        sendNewFields=(values,bundleId)=>{
            var body = {
                name: values.FieldName,
                FieldTypeId: values.FieldTypeId,
                bundleId: bundleId,
                description: values.Description
            };
            http.HttpAjax({
               url: DataURL + '/api/BundleField',
               method: 'POST',
               data:body,
              headers: {
                'Content-Type': 'application/json',
                'authorization':'Bearer ' + localStorage.access_token
              },
            }).then((data)=>{
                this.setState({iconLoading:false})
                let path = `/Structure/Bundles/fields?bundleId=${this.bundleId}`
                this.context.router.push(path)
            }).catch((error) => {
                console.error(error);
            });
        }


        render(){
            const { getFieldDecorator } = this.props.form;
            const formItemLayout = {
              labelCol: { span: 6 },
              wrapperCol: { span: 14 },
            };
            const FieldTypes = this.state.optionValue.map(value=><Option key={value.fieldTypeId}>{value.fieldTypeName}</Option>)
            return(
              <div className='NewForm'>
                  <Form onSubmit={this.handleSubmit}>
                      <FormItem
                            {...formItemLayout}
                            label='Field Name '
                            hasFeedback
                      >
                        {getFieldDecorator('FieldName', {
                          rules: [{type:'string',message:'The input is not valid string!',whitespace:true},
                          { required: true, message: 'Please input your Field Name!' }],
                        })(
                          <Input  placeholder="Field Name" />
                        )}
                      </FormItem>
                      <FormItem
                            {...formItemLayout}
                            label='Description '
                            hasFeedback
                      >
                        {getFieldDecorator('Description', {
                          rules: [{type:'string',message:'The input is not valid string!',whitespace:true},
                          { required: true, message: 'Please input your Description!' }],
                        })(
                          <Input  placeholder="Description" />
                        )}
                      </FormItem>
                      <FormItem
                        {...formItemLayout}
                        label='Select '
                        hasFeedback
                      >
                        {getFieldDecorator('FieldTypeId',{
                            rules:[
                              { required: true, message: 'Please select your Field Type!'}
                            ]
                        })(
                          <Select placeholder="Please select a Field Type" >
                              {FieldTypes}
                          </Select>
                        )}
                      </FormItem>
                      <FormItem
                        wrapperCol={{ span: 12, offset: 6 }}
                      >
                        <Button type="primary" htmlType="submit">Submit</Button>
                      </FormItem>
                  </Form>

              </div>
            );
        }
    }

    const NewFieldsForm = Form.create()(NewFields);
    NewFields.contextTypes = {
      router: React.PropTypes.object
    }
    export default NewFieldsForm
