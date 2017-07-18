import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select} from 'antd';
import Http from '../../../components/XmlHttp';
import {DataURL} from '../../../config/DataURL-Config'
import AutoCompompleteInput from '../../../components/AutoCompompleteInput'
const http = new Http();

export default class TaxonomyContainer extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      filtered: false,
      tData : [],
      loading : true,
      constData : []
    }
  }

  componentDidMount = () => {
    this.fetchFn();
  }

  fetchFn = () => {
          http.HttpAjax({
              url: DataURL + '/api/Taxonomy'
          }).then((data)=>{
              this.setState({tData:data, loading:false, constData : data});
          }).catch((e)=>{
                  console.log(e.message)
          })
  }

  onInputChange = (e) => {
     this.setState({ searchText: e.target.value });
   }

  onSearch = (searchText) => {
     const reg = new RegExp(searchText, 'gi');
     this.setState({
       filtered: !!searchText,
       tData: this.state.constData.map((record) => {
         const match = record.name.match(reg);
         if (!match) {
           return null;
         }
         return {
           ...record,
           name: (
             <span>
               {record.name.split(reg).map((text, i) => (
                 i > 0 ? [<span className="highlight">{match[0]}</span>, text] : text
               ))}
             </span>
           ),
         };
       }).filter(record => !!record),
     });
   }


  render() {
    const columns = [
      {
        title : 'Name',
        dataIndex : 'name',
        key : 'name'
      },
      {
        title : 'Description',
        dataIndex : 'description',
        key : 'description'
      },
      {
        title : 'Entity Name',
        dataIndex : 'entityName',
        key : 'entityName'
      },
      {
        title : 'Status',
        dataIndex : 'status',
        key : 'status'
      },
      {
        title : 'Action',
        key : 'bundleId',
        dataIndex : 'bundleId',
        render: (text, record) => (
          <span>
            <Link to={{pathname:"Structure/Bundles/fields", query:{bundleId : text}}}>Fields config</Link>
            <span className="ant-divider" />
            <Link onClick={e => (e.preventDefault(), alert(JSON.stringify(record)))}>Details</Link>
            <span className="ant-divider" />
            <Link to={{pathname:"Structure/Taxonomy/TaxonomyTerm", query:{bundleId:text}}}>Terms</Link>
          </span>
        )
      }
    ];
    const inputStyle = {
      width : 300,
      position : 'relative',
      left : '65%'
    }
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
      <div>
        <div style={{width:'80%', marginTop:'2%', marginLeft:'10%'}}>
          <div>
            <AutoCompompleteInput onSelectCallBack={this.onSearch} dataSource={this.state.constData} clickSearchCallBack={this.onSearch}/>
          </div>
        </div>

        <div className='table' style={{width:'90%', marginTop:'2%', marginLeft:'5%'}}>
            <Table {...this.state} rowKey={record => record.id} dataSource={this.state.tData} columns={columns} pagination={pagination} />
        </div>
      </div>
    );
  }
}
