import React from 'react'
import {Link} from 'react-router'
import {
  Form, Select, InputNumber, Switch, Radio,
  Slider, Button, Upload, Icon,Input
} from 'antd';
const FormItem = Form.Item;
const Option = Select.Option;
const options = [
  {
    triggerName : 'Entity Created',
    triggerId : 1
  },
  {
    triggerName : 'Entity Deleted',
    triggerId : 2
  },
]
export class NewRulesContainer extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      iconLoading : false
    }
  }
  handleSubmit = (e) => {
    e.preventDefault();
    this.props.form.validateFields((err, values) => {
      console.log(values);
      this.context.router.push('Structure/Rules/RulesCondition');
    });
  }
  render() {
    const { getFieldDecorator } = this.props.form;
    const formItemLayout = {
      labelCol: { span: 6 },
      wrapperCol: { span: 14 },
    };
    const TriggerEvent = options.map(value=><Option key={value.triggerId}>{value.triggerName}</Option>)
    return (
      <div className='NewForm'>
          <Form onSubmit={this.handleSubmit}>
              <FormItem
                    {...formItemLayout}
                    label='Trigger Name '
                    hasFeedback
              >
                {getFieldDecorator('TriggerName', {
                  rules: [{type:'string',message:'The input is not valid string!',whitespace:true},
                  { required: false, message: 'Please input your Trigger Name!' }],
                })(
                  <Input  placeholder="Trigger Name" />
                )}
              </FormItem>

              <FormItem
                {...formItemLayout}
                label='Trigger Event'
                hasFeedback
              >
                {getFieldDecorator('TriggerEvent',{
                    rules:[
                      { required: false, message: 'Please select your Trigger Event!'}
                    ]
                })(
                  <Select placeholder="Please select a Trigger Event" >
                      {TriggerEvent}
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
    )
  }

}
const NewRulesForm = Form.create()(NewRulesContainer);
NewRulesContainer.contextTypes = {
  router: React.PropTypes.object
}
export default NewRulesForm
