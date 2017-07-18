import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form} from 'antd';
import Http from '../../../components/XmlHttp';
import {DataURL} from '../../../config/DataURL-Config'
const http = new Http();

import  {getUrlParams} from '../../../components/Utils.js'
const FormItem = Form.Item;
 export default class FieldsContainer extends React.Component{
   constructor(props){
        super(props);
        this.state = {
            tData:[],
            count:3  ,
            loading:true,
            visibleModal:false,
            bundleId:''
        }
         let wurl = window.location.href;
         let bundleId =getUrlParams(wurl).bundleId;
         this.geturl(bundleId);
    }


    geturl(bundleId){
        this.fetchFn(bundleId)
    }

    fetchFn = (bundleId) => {
        http.HttpAjax({
            url: DataURL + '/api/BundleField/' +bundleId + '/Fields'
        }).then((data)=>{
               this.setState({tData:data})
               this.setState({loading:false})
               this.setState({bundleId:bundleId})
        }).catch((e) => {
            console.log(e.message)
        })
    }


    handleToggle = (prop) => {
        return (enable) => {
        this.setState({ [prop]: enable });
        };
    }




    onCellChange = (index, key) => {
        return (value) => {
        const dataSotDataurce = [...this.state.tData];
        tData[index][key] = value;
        this.setState({ tData });
        };
    }
    onDelete = (index) => {
        const tData = [...this.state.tData];
        tData.splice(index, 1);
        this.setState({ tData });
    }
    handleAdd = () => {
        const count = this.state.count;
        const tData = this.state.tData;
        const newData = {
            key: count,
            'fieldName': `1${count}`,
            'fieldType': '2',
            'entityName': `3`,
            'bundleName':'4',
            'status':'5'
        };
        this.setState({
            tData: [...tData, newData],
            count: count + 1,
        });
    }

    render(){
        const  tdata  = this.state.tData;
        const columns = [{
            title:'Name',
            dataIndex:'fieldName',
            key:'fieldName',
        }, {
            title:'Field Type',
            dataIndex:`fieldTypeName`,
            key:'fieldType'
        }, {
            title: 'Entity Name',
            dataIndex: 'entityName',
            key: 'entityName',
        }, {
            title:'Bundle Name',
            dataIndex:'bundleName',
            key:'bundleName'
        },{
            title:'Status',
            dataIndex:'status',
            key:'status'
        },{
            title: 'Action',
            key: 'action',
            render: (text, record, index) => {
                return (
                this.state.tData.length > 1 ?
                (
                    <Popconfirm title="Sure to delete?" onConfirm={() => this.onDelete(index)}>
                    <a href="#">Delete</a>
                    </Popconfirm>
                ) : null
                );
            },
        }];


    var style = {
        'margin-left':20
    }

        return(
            <div className='table' style={{width:'90%', marginTop:'2%', marginLeft:'5%'}}>
                <Table {...this.state} rowKey={record => record.fieldName}  dataSource={tdata} columns={columns} />
                <Button type='primary'><Link to={{pathname:'Structure/Bundles/NewFields',query:{bundleId:this.state.bundleId}}} >New Field</Link></Button>
            </div>
        )
    }

 }
