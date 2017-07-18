import React from 'react'
import {Link} from 'react-router';
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select} from 'antd';
import Http from '../../../components/XmlHttp';
import {DataURL} from '../../../config/DataURL-Config'
const http = new Http();
const Option = Select.Option;
export default class RecordContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
        tData:[],
        count:3  ,
        iconLoading:true,
        visibleModal:false,
        bundleId:null,
        bundleIds : []
    }
    this.handleChange = this.handleChange.bind(this);
  }

  fetchFn = (bundleId) => {
    if(bundleId.length != 0){
      http.HttpAjax({
          url: DataURL + '/api/Node/' + bundleId
      }).then((data)=>{
             this.setState({
               tData : data,
               loading : false,
               bundleId : bundleId
             })
      }).catch((e) => {
          console.log('Error',e.message);
      })
    }
  }
  fetchSelection = () => {
    http.HttpAjax({
        url: DataURL + '/api/Bundle',
    }).then((data)=>{
        this.setState({bundleIds:data, iconLoading:false});
    }).catch((e)=>{
            console.log(e.message)
    })
  }

  componentDidMount() {
      this.fetchSelection();
  }

  handleChange(value) {
    this.setState({iconLoading:true})
    this.fetchFn(value);
  }

  render() {
    const tdata = this.state.tData;
    const columns = [
      {
        title : 'Record Name',
        dataIndex : 'name',
        key : 'name'
      },
      {
        title : 'Entity Name',
        dataIndex : 'entityName',
        key : 'entityName'
      },
      {
          title: 'Description',
          dataIndex: 'description',
          key: 'description'
      },
      {
          title: 'Status',
          dataIndex: 'status',
          key: 'status'
      }
    ];

    const myStyle = {
      width : '150px',
      position : 'relative',
      marginLeft :'80%',
      marginTop:'2%'
    }

    const myBundleId = this.state.bundleIds.map(value=><Option key={value.id} >{value.name}</Option>)
    return (
      <div>
        <Select style={myStyle} onChange={this.handleChange} placeholder='Select Bundle'>
          {myBundleId}
        </Select>
        <div className='table' style={{width:'90%', marginTop:'2%', marginLeft:'5%'}}>
            <Table {...this.state} rowKey={record => record.fieldName} dataSource={tdata} columns={columns} />
            {this.state.bundleId && <Link to={{pathname:'Structure/Bundles/AddRecord', query:{bundleId:this.state.bundleId}}}><Button type='primary' style={{marginTop:'2%'}}>New Record</Button></Link>}
        </div>
      </div>
    )
  }


}
