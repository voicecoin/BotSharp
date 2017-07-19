import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox,Button } from 'antd';
import '../../Sources/style/GridResize.css';
import '../../Sources/style/GridLayout.css';
import {Responsive, WidthProvider} from 'react-grid-layout';
const ResponsiveReactGridLayout = WidthProvider(Responsive);
export default class PageLayout extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      cards:[],
      number : 0,
      layout : []
    }
  }
  onLayoutChange = (layout, allLayout) => {
    this.setState({
      layout : layout
    })
  }

  addCard = () => {
    this.setState((prev, props) => {
      cards : prev.cards.push({key:prev.number, x: prev.cards.length % 12, y: Infinity, w: 1, h: 1})
    });
    this.setState((prev, props) => ({
      number : prev.number + 1
    }));

  }

  deleteCard = (key) => {
    for(var i = 0; i < this.state.cards.length; i ++){
      if(this.state.cards[i].key == key){
        this.setState((prev, props) => {
          cards : prev.cards.splice(i, 1)
        });
        return ;
      }
    }
  }
  submit = () => {
    let data = [];
    for(var i in this.state.layout){
      let temp = this.state.layout[i];
      let obj = {
        x : temp.x,
        y : temp.y,
        width : temp.w,
        height : temp.h,
        key : temp.i
      };
      data.push(obj);
    }
    console.log(data);
    this.setState({cards : data})
  }

  render() {
    const removeStyle = {
      position: 'absolute',
      right: '2px',
      top: 0,
      cursor: 'pointer'
    };
    const cards = this.state.cards.map((values) =>
      <div key={values.key} data-grid={{w: values.w, h: values.h, x: values.x, y: values.y}} style={{backgroundColor:'grey'}}>
         <span className="remove" style={removeStyle} onClick={() => this.deleteCard(values.key)}>x</span>
      </div>
    )
    return (
      <div style={{width:'90%', marginLeft:'4.5%', marginTop:'2%'}}>
        <Button type='primary' onClick={this.addCard}>Add Card</Button>
        <Button type='primary' onClick={this.submit} style={{marginLeft:'2%'}}>Submit</Button>
        <ResponsiveReactGridLayout className="layout"
         breakpoints={{lg: 1200, md: 996, sm: 768, xs: 480, xxs: 0}}
         cols={{lg: 12, md: 10, sm: 6, xs: 4, xxs: 2}}
         onLayoutChange={this.onLayoutChange}
         >
         {cards}
       </ResponsiveReactGridLayout>
     </div>
    )
  }
}
