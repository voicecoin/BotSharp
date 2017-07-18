import React from 'react'
import {
    Form, Select,Button, Icon,Input
} from 'antd';
import {DataURL} from '../../../config/DataURL-Config'
const FormItem = Form.Item;
const Option = Select.Option;
import Http from '../../../components/XmlHttp'
const http = new Http();
    export  class NewBundle extends React.Component{
        constructor(props){
            super(props)
            this.state={
                optionValue:[],
                iconLoading:false
            }
            this.fetchOptionValue();
        }

        fetchOptionValue=()=>{
            http.HttpAjax({
                url: DataURL + '/api/Entity/Bundlables'
            }).then((data) => {
                this.setState({optionValue:data})
            }).catch((e)=>{console.log(e.message)})
        }

        handleSubmit= (e)=>{
            e.preventDefault();
            this.setState({
                iconLoading:true
            })
            this.props.form.validateFields((err,values)=>{
                if(!err){
                    this.sendNewBundle(values)
                }
            })
        }


        sendNewBundle = (value)=>{
            var body ={
                name: value.BundleName,
                entityName: value.EntityName,
                description: value.Description
            }
            http.HttpAjax({
                url: DataURL + '/api/Bundle',
                method:'POST',
                headers: {
                'Content-Type': 'application/json',
                'authorization':'Bearer ' + localStorage.access_token
              },
                data:body
            }).then((data)=>{
                this.setState({
                    iconLoading:false
                })
                let path = `/Structure/Bundles`
                this.context.router.push(path)
            }).catch((error) => {
                console.error(error);
            })
        }

        render(){
            const { getFieldDecorator } = this.props.form;
            const formItemLayout = {
              labelCol: { span: 6 },
              wrapperCol: { span: 14 },
            };
            const EntityTypes = this.state.optionValue.map(value=><Option key={value}>{value}</Option>)
            return(
                <div className='NewForm'>
                    <Form  onSubmit={this.handleSubmit}>
                        <FormItem
                            {...formItemLayout}
                            label='Bundle Name'
                            hasFeedback
                        >
                            {getFieldDecorator('BundleName', {
                            rules: [{type:'string',message:'The input is not valid string!',whitespace:true},
                            { required: true, message: 'Please input your Field Name!' }],
                            })(
                            <Input  placeholder="Bundle Name" />
                            )}
                        </FormItem>
                        <FormItem
                            {...formItemLayout}
                            label='Description'
                            hasFeedback
                        >
                            {getFieldDecorator('Description',{
                                rules:[{type:'string',message:'The input is not valid string!',whitespace:true},
                                {required:true,message:'Please input your Description!'}],
                            })(
                                <Input placeholder='Description'/>
                            )}

                        </FormItem>
                        <FormItem
                            {...formItemLayout}
                            label='Select '
                            hasFeedback
                        >
                            {getFieldDecorator('EntityName',{
                                rules:[
                                    {required:true,message:'Please select your Bundle Type!'}
                                ]
                            })(
                                <Select placeholder='Please Select a Bundle Type'>
                                    {EntityTypes}
                                </Select>
                            )}
                        </FormItem>
                        <FormItem
                            wrapperCol={{span:12,offset:6}}
                        >
                            <Button type='primary' htmlType='submit' loading={this.state.iconLoading}>Submit</Button>
                        </FormItem>
                    </Form>
                </div>
            )
        }
    }
    const NewBundleForm = Form.create()(NewBundle);
    NewBundle.contextTypes = {
      router: React.PropTypes.object
    }
    export default NewBundleForm
