import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button, Table} from 'antd';
const FormItem = Form.Item;
import {DataURL} from '../../config/DataURL-Config';
import Http from '../XmlHttp'
const http = new Http();
export class TableView extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : this.props.data
    }
    console.log(props.data);
  }

  handleAction = (url, method) => {
    http.HttpAjax({
        url: DataURL + url,
        method : method
    }).then((data)=>{
        this.setState({tData:data,loading:false});
    }).catch((e)=>{
      console.log(e.message)
    })
  }


  render() {
    var columns = [];
    for(var i in this.state.data[0].columns){
      let data = this.state.data[0].columns[i];
      if(data.fieldType == 4){
        columns.push({
          title : data.displayName,
          key : data.fieldName,
          fieldType : data.fieldType,
          dataIndex : data.fieldName,
          colSpan : 0.001,
          render : (text, record) => (
            <img src={record.avatar} style={{width:'10%'}}></img>
          )
        })
      }
      else{
        columns.push({
          title : data.displayName,
          key : data.fieldName,
          dataIndex : data.fieldName,
          fieldType : data.fieldType
        });
      }

    }
    columns.push({
      title : 'Action',
      key : 'action',
      render : (text ,record) => (
          this.state.data[0].actions.map((values) => {

            if(values.redirectUrl && values.requestUrl){
              return (
                <Link to={{pathname:values.redirectUrl, query:record.id}} key={values.name} onClick={() => this.handleAction(values.requestUrl, values.requestMethod)}>{values.name}<span className="ant-divider" /></Link>
              )
            }
            else if(values.redirectUrl && !values.requestUrl){
              return (
                <Link to={{pathname:values.redirectUrl, query:record.id}} key={values.name}>{values.name}<span className="ant-divider" /></Link>
              )
            }
            else if(!values.redirectUrl && values.requestUrl){
              return (
                <Link key={values.name} onClick={() => this.handleAction(values.requestUrl, values.requestMethod)}>{values.name}<span className="ant-divider" /></Link>

              )
            }
          })
      )
    });
    return (
      <div style={{width:'90%', marginTop:'2%', marginLeft:'5%'}}>
        <Table rowKey={(record) => {return record.id}} columns={columns} dataSource={this.state.data[0].data} />
      </div>
    )
  }
}

const TableViewForm = Form.create()(TableView);
Table.contextTypes = {
  router : React.PropTypes.object
}
export default TableViewForm
