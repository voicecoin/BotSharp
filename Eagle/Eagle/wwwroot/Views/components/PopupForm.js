import { Button, Modal, Form, Input, Radio ,Select } from 'antd';
import React from 'react'
const FormItem = Form.Item;
const Option = Select.Option;
const RadioButton = Radio.Button;
const RadioGroup = Radio.Group;


const CollectionCreateForm = Form.create()(
  (props) => {
    const { visible, onCancel, onCreate, form } = props;
    const { getFieldDecorator } = form;
    const formItemLayout = {
      labelCol: { span: 6 },
      wrapperCol: { span: 14 },
    };
    return (
      <Modal
        visible={visible}
        title="Create a new Fields"
        okText="Create"
        cancelText='Cancel'
        onCancel={onCancel}
        onOk={onCreate}
      >
        <Form layout="vertical">
            <FormItem
            {...formItemLayout}
            label="Select Entity type"
            hasFeedback
            >
            {getFieldDecorator('select', {
                rules: [
                { required: true, message: 'Please select your country!' },
                ],
            })(
                <Select placeholder="Please select entity type">
                <Option value="china">China</Option>
                <Option value="use">U.S.A</Option>
                </Select>
            )}
            </FormItem>

            <FormItem
            {...formItemLayout}
            label="Bundle Name"
            >
            {getFieldDecorator('select-multiple', {
                rules: [
                { required: true, message: 'Please select your favourite colors!', type: 'array' },
                ],
            })(
                <Select mode="multiple" placeholder="Please input Bundle Name">
                <Option value="red">Red</Option>
                <Option value="green">Green</Option>
                <Option value="blue">Blue</Option>
                </Select>
            )}
            </FormItem>         
        </Form>
      </Modal>
    );
  }
);

class PopupForm extends React.Component {
  state = {
    visible: false,
  };
  showModal = () => {
    this.setState({ visible: true });
  }
  handleCancel = () => {
    this.setState({ visible: false });
  }
  handleCreate = () => {
    const form = this.form;
    form.validateFields((err, values) => {
      if (err) {
        return;
      }

      console.log('Received values of form: ', values);
      form.resetFields();
      this.setState({ visible: false });
    });
  }
  saveFormRef = (form) => {
    this.form = form;
  }
  render() {
    return (
      <div>
        <CollectionCreateForm
          ref={this.saveFormRef}
          visible={this.state.visible}
          onCancel={this.handleCancel}
          onCreate={this.handleCreate}
        />
      </div>
    );
  }
}

export default PopupForm;