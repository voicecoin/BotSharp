import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button } from 'antd';
const FormItem = Form.Item;
import Http from '../../components/XmlHttp';
import {DataURL} from '../../config/DataURL-Config';
const http = new Http();
import {View} from '../../components/View/View'
import { Responsive, WidthProvider } from 'react-grid-layout';

import  ReactGridLayout from 'react-grid-layout';
const ResponsiveReactGridLayout = WidthProvider(Responsive);

export class SharedContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      tData : null
    }
  }

  fetchFn = () => {
    http.HttpAjax({
        url: DataURL + '/api/Page/' + this.props.params.value,
        headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')}
    }).then((data)=>{
      console.log(data);
      this.setState({tData : data});
    }).catch((e)=>{
        console.log(e.message)
    })
  }
  componentDidMount() {
      this.fetchFn();
  }
  render() {
    const data = this.state.tData && this.state.tData.blocks.map((values) =>
      <div key={values.id} data-grid={{w: values.position.width, h: values.position.height, x: values.position.x, y: values.position.y}} style={{backgroundColor:'white'}}>
         <View DataUrl={values.dataUrl}/>
      </div>
    )
    return (
      <div style={{width:'90%', marginLeft:'4.5%', marginTop:'2%'}}>
         <ReactGridLayout className="layout" width={1500} isDraggable={false} isResizable={false}>
         {data}
       </ReactGridLayout>
     </div>
    )

  }
}

const SharedContainerForm = Form.create()(SharedContainer);
SharedContainer.contextTypes = {
  router : React.PropTypes.object
}
export default SharedContainerForm
