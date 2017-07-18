import React from 'react';
import $ from 'jquery';
import { Table, Icon, Switch, Radio, Form,Button  } from 'antd';
import { Router, Route, Link, hashHistory, IndexRoute, Redirect, IndexLink} from 'react-router';
import {DataURL} from '../../../config/DataURL-Config';
import Http from '../../../components/XmlHttp'
const http = new Http();
const columns = [{
    title:'Name',
    dataIndex:'name',
    key:'name',
    width:150,
}, {
    title:'Entity Name',
    dataIndex:'entityName',
    key:'entityName'
}, {
    title: 'Status',
    dataIndex: 'status',
    key: 'status',
}, {
  title: 'Action',
  key: 'action',
  render: (text, record) => (
    <span>
      <Link to={{pathname:"Structure/Bundles/fields",query:{bundleId:record.id}}}>Fields config</Link>
      <span className="ant-divider" />
      <Link to={{pathname:"Structure/Bundles/AddRecord",query:{bundleId:record.id}}}>Add Record</Link>
      <span className="ant-divider" />
      <a href="javascript:">Remove</a>
    </span>
  ),
}];

export default class HomeContainer extends React.Component{
    constructor(props){
        super(props);
        this.state={
            tData:[],
            loading:true
        }
    }

    fetchFn = () => {
            http.HttpAjax({
                url: DataURL + '/api/Bundle',
                headers:{'authorization':'Bearer ' + localStorage.getItem('access_token')}
            }).then((data)=>{
                this.setState({tData:data,loading:false});
            }).catch((e)=>{
                    console.log(e.message)
            })


    }
    componentDidMount() {
        this.fetchFn()
    }

    handleToggle = (prop) => {
        return (enable) => {
            this.setState({ [prop]: enable });
        };
    }

    render(){
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
            <Table {...this.state} rowKey={record => record.id}   columns={columns} dataSource={state.tData} pagination={pagination} />
             <Link to='Structure/Bundles/NewBundle'><Button type='primary'>New Bundle</Button></Link>
         </div>
       )
    }

}
