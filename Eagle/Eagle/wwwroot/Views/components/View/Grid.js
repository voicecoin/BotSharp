import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button, Card, Pagination } from 'antd';
const FormItem = Form.Item;
export class Grid extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : [],
      currentPage : 0,
    }
  }
  componentWillMount = () => {
    this.handleData();
  }
  handleData = () => {
    let data = null;
    for(var i in this.props.data){
      if(this.props.data[i].dataType == 'Grid'){
        data = this.props.data[i];
      }
    }
    if(data != null){
      let stateData = [];
      let onePage = [];
      for(var i = 0; i < data.data.length; i ++){
        if(i != 0 && i % 6 == 0){
          stateData.push(onePage);
          onePage = [{name:data.data[i].name, key:i}];
        }
        else{
          onePage.push({name:data.data[i].name, key:i});
        }
      }
      stateData.push(onePage);
      this.setState({
        data: stateData
      })
    }
  }
  handleAction = () =>{
    console.log('enter')
  }
  onChange = (page, pageSize) => {
    this.setState({
      currentPage : page - 1
    })
  }


  render() {
    const imgStyle = {
      textAlign:'center'
    };
    const bodyStyle = {
      textAlign:'center'
    };
    const botStyle = {
      textAlign:'center'
    };
    const cards = this.state.data[this.state.currentPage].map(values =>
        <Col span={6} offset={1} key={values.key}>
          <Card style={{ width: 240, marginTop:'5%' }} bodyStyle={{ padding: 5 }}>
            <div style={imgStyle}>
              <img width="80%" src="https://picturethismaths.files.wordpress.com/2016/03/fig6bigforblog.png?w=419&h=364" />
            </div>
            <hr/>
            <div style={bodyStyle}>
              <h5>{values.name}</h5>
            </div>
            <hr/>
            <div style={botStyle}>
                <Link onClick={this.handleAction}>Action</Link>
                <span className="ant-divider" />
                <Link onClick={this.handleAction}>Delete</Link>
            </div>
          </Card>
        </Col>
    );

    return (
      <div style={{marginTop:'3%', marginLeft:'5%'}}>
        <div style={{marginTop:'3%', marginLeft:'7%'}}>
          <Row gutter={16}>
            {cards}
          </Row>
        </div>
        <div style={{marginLeft:'40%', marginTop:'5%'}}>
          <Pagination simple defaultCurrent={1} total={6 * this.state.data.length} pageSize={6} onChange={this.onChange} current={this.state.currentPage + 1}/>
        </div>
      </div>
    )
  }
}

const GridForm = Form.create()(Grid);
Grid.contextTypes = {
  router : React.PropTypes.object
}
export default GridForm
