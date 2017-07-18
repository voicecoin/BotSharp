import React from 'react';
import { Modal, Button } from 'antd';

export  class Popthewindow extends React.Component{
        constructor(props){
            super(props)
            this.state={
                ModalContent:this.props.content,
                visible:false
            }
                
        }

        showModal = () => {
            this.setState({
                visible: true,
            });
        }      

        handleOk = () => {
            this.setState({
                 confirmLoading: true,
            });
            
            setTimeout(() => {
                this.setState({
                    visible: false,
                    confirmLoading: false,
                });
                this.props.callbackhandleAdd()
            }, 2000);
        }
        handleCancel = () => {
            console.log('Clicked cancel button');
            this.setState({
            visible: false,
            });
        }      


        render(){
            const   { visible, confirmLoading, ModalContent } = this.state;
            return (
                <div>
                    <Modal title={this.props.title}
                    visible={visible}
                    onOk={this.handleOk}
                    okText="Create"
                    cancelText='Cancel'
                    confirmLoading={confirmLoading}
                    onCancel={this.handleCancel}
                    >
                        {ModalContent}
                    </Modal>
                </div>
            );        
        }  
}

export default Popthewindow