import React from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select} from 'antd';
import {Link} from 'react-router';
import {RuleHelper} from '../../../components/RuleHelper';
const filterNames = ['name1', 'name2', 'name3', 'name4'];

const filterOperations = ['EQUAL TO', 'NON-EQUAL TO'];

const filterValues = ['value1','value2','value3','value4','value5',];

const filterConnections = ['OR', 'AND', 'NOR', 'XOR'];
export class RulesConditionContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      tData : {}
    }
  }

  getCallback = (parentData, subData) => {
    let data = {};
    for(var i = 0; i < parentData.length; i ++){
      data[parentData[i].id] = parentData[i];
      data[parentData[i].id].children = subData[parentData[i].id] ? subData[parentData[i].id] : [];
    }
    console.log(data);
    this.setState({tData : data})
  }


  render() {

    return (
      <div>
        <RuleHelper Callback={this.getCallback}/>
      </div>
    )
  }
}

const RulesConditionForm = Form.create()(RulesConditionContainer);
RulesConditionContainer.contextTypes = {
  router: React.PropTypes.object
}
export default RulesConditionForm;
