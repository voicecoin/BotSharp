import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select} from 'antd';
import {Link} from 'react-router';
const FormItem = Form.Item;
const Option = Select.Option;
import '../Sources/style/react-bootstrap-table-all.min.css'
const filterNames = [undefined, 'name1', 'name2', 'name3', 'name4'];

const filterOperations = [undefined, 'EQUAL TO', 'NON-EQUAL TO'];

const filterValues = [undefined, 'value1','value2','value3','value4','value5',];

const filterConnections = [undefined, 'OR', 'AND', 'NOR', 'XOR'];
var subData = {};
export class RuleHelper extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      parentData : []
    }

  }

  expandColumnComponent({ isExpandableRow, isExpanded }) {
    let content = '';
    if (isExpandableRow) {
      content = (isExpanded ? '➖' : '➕' );
    } else {
      content = ' ';
    }
    return (
      <div> { content } </div>
    );
  }

  expandComponent(row) {

    const cellEditProp = {
      mode: 'click',
      blurToSave: true
    };
    const type = ['Rule', 'Operation']
    if(row.type == 'Rule'){
      return (
      <BootstrapTable data={ subData[row.id] ? subData[row.id] : [] }
          insertRow={true}
          cellEdit={ cellEditProp }
          options={{expandBy:'column', afterDeleteRow: (data) => {
            let obj = subData[row.id];
            for(var i = 0; i < data.length; i ++){
              for(var j = 0; j < obj.length; j ++){
                if(obj[j].id == data[i]){
                  obj.splice(j, 1);
                }
              }
            }
            subData[row.id] = obj;
          }, afterInsertRow: (data) => {if(!subData[row.id]) subData[row.id] = []; subData[row.id].push(data)}}}
          deleteRow={true}
          selectRow={{mode:'checkbox',clickToExpand:true}}
          >
        <TableHeaderColumn dataField='id' isKey={ true }>ID</TableHeaderColumn>
        <TableHeaderColumn dataField='name' editable={{type : 'select', options:{values : filterNames}}}>Filter Name</TableHeaderColumn>
        <TableHeaderColumn dataField='op' editable={{type : 'select', options:{values : filterOperations}}}>Operation Name</TableHeaderColumn>
        <TableHeaderColumn dataField='value' editable={{type : 'select', options:{values : filterValues}}}>Filter Value</TableHeaderColumn>
        <TableHeaderColumn dataField='connector' editable={{type : 'select', options:{values : filterConnections}}}>Connector</TableHeaderColumn>
      </BootstrapTable>
      );
    }
    else{
      return (
        <BootstrapTable data={ subData[row.id] ? subData[row.id] : [] }
            insertRow={true}
            cellEdit={ cellEditProp }
            options={{expandBy:'column', afterDeleteRow: (data) => {
              let obj = subData[row.id];
              for(var i = 0; i < data.length; i ++){
                for(var j = 0; j < obj.length; j ++){
                  if(obj[j].id == data[i]){
                    obj.splice(j, 1);
                  }
                }
              }
              subData[row.id] = obj;
            },
            afterInsertRow: (data) => {if(!subData[row.id]) subData[row.id] = []; subData[row.id].push(data)}}}
            deleteRow={true}
            selectRow={{mode:'checkbox',clickToExpand:true}}
            >
          <TableHeaderColumn dataField='id' isKey={ true }>ID</TableHeaderColumn>
          <TableHeaderColumn dataField='connector' editable={{type : 'select', options:{values : filterConnections}}}>Filter Connection</TableHeaderColumn>
        </BootstrapTable>
      )
    }

  }

  isExpandableRow(row) {
    return true;
  }

  handleInsertedRow = (row) => {
    this.setState((prev) => {
      parentData : prev.parentData.push(row)
    })
  }

  handleDeletedRow = (rowKeys) => {
    for(var j = 0; j < rowKeys.length; j ++){
      let obj = this.state.parentData;
      for(var i = 0; i < obj.length; i ++){
        if(obj[i].id == rowKeys[j]){
          obj.splice(i, 1);
          this.setState({
            parentData : obj
          });
          break;
        }
      }
    }
  }

  handleSubmit = () => {
    let newdata = {};
    for(var i in subData){
      let arr = subData[i];
      let newarr = [];
      for(var j = 0; j < arr.length; j ++){
        let temp = {};
        if(arr[j].name){
          temp['id'] = arr[j].id;
          if(arr[j].name) temp['name'] = arr[j].name;
          if(arr[j].op) temp['op'] = arr[j].op;
          if(arr[j].value) temp['value'] = arr[j].value;
          newarr.push(temp);
          if(arr[j].connector) newarr.push({connector : arr[j].connector})
        }
        else{
          temp['id'] = arr[j].id;
          temp['connector'] = arr[j].connector;
          newarr.push(temp);
        }
      }
      newdata[i] = newarr;
    }
    this.props.Callback(this.state.parentData, newdata);
  }

  render() {

    const cellEditProp = {
      mode: 'click',
      blurToSave: true
    };
    const type = ['Rule', 'Operation'];
    const buttonStyle = {
      position:'relative',
      marginTop:'5%',
      marginLeft:'80%'
    }
    return (
      <div style={{width:'90%', position:'relative', marginLeft:'3%', marginTop:'5%'}}>
        <BootstrapTable data={ this.state.parentData }
          expandableRow={this.isExpandableRow}
          expandComponent={ this.expandComponent }
          expandColumnOptions={ {
            expandColumnVisible: true,
            expandColumnComponent: this.expandColumnComponent,
            columnWidth: 50
          } }
          deleteRow={true}
          selectRow={{mode:'checkbox',clickToExpand:true}}
          insertRow={true}
          cellEdit={ cellEditProp }
          options={{expandBy:'column', afterDeleteRow: this.handleDeletedRow, afterInsertRow: this.handleInsertedRow}}
          >
          <TableHeaderColumn dataField='id' expandable={false} isKey={ true }>ID</TableHeaderColumn>
          <TableHeaderColumn dataField='type' expandable={false} editable={{type : 'select', options:{values : type}}}>Type</TableHeaderColumn>
          <TableHeaderColumn dataField='name' expandable={false} >Name</TableHeaderColumn>
        </BootstrapTable>
        <Button type='primary' onClick={this.handleSubmit} style={buttonStyle}>Submit</Button>
      </div>
    );
  }
}

const RuleForm = Form.create()(RuleHelper);
RuleHelper.contextTypes = {
  router : React.PropTypes.object
}
export default RuleForm;
