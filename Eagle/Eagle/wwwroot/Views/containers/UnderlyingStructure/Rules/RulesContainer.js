import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select} from 'antd';
const columns = [{
    title:'Name',
    dataIndex:'name',
    key:'name'
}, {
    title:'Trigger',
    dataIndex:'trigger',
    key:'trigger'
}, {
    title: 'Detail',
    dataIndex: 'detail',
    key: 'detail',
}, {
  title: 'Action',
  key: 'action',
  render: (text, record) => (
    <span>
      <a href="#">Delete Rule</a>
    </span>
  ),
}];


export default class RulesContainer extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      tData:[],
      loading:false
    }
  }
  render() {
    const state = this.state;
    const pagination = {
        total: this.state.tData.length,
        showSizeChanger: true,
        onShowSizeChange(current, pageSize) {
             console.log('Current: ', current, '; PageSize: ', pageSize)
         },
        onChange(current) {
             console.log('Current: ', current)
         }
    };
   return(
     <div className='table' style={{width:'90%', marginTop:'2%', marginLeft:'5%'}}>
        <Table {...this.state} rowKey={record => record.id} columns={columns} dataSource={state.tData} pagination={pagination} />
         <Link to='Structure/Rules/NewRules'  ><Button type='primary' style={{'marginTop':'1%'}}>New Rule</Button></Link>
     </div>
   )
  }

}
