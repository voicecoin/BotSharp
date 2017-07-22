import React from 'react'
import {Link} from 'react-router'
import { Table, Input, Icon, Button, Popconfirm,Switch,Form, Select, Modal, Row, Col} from 'antd';
import TreeView from 'treeview-react-bootstrap';
const FormItem = Form.Item;
const Option = Select.Option;

export class ScenesContainer extends React.Component {
  constructor(props){
    super(props);
    this.state = {

    }
  }
  componentDidMount = () => {
    this.draw();
  }
  draw = () => {
    var circles = [50, 100];
    var lines = [{x:100, y:100}, {x:300, y:300}, {x:400, y:400}];

    //define canvas
    var canvas = d3.select("#canvas")
      .append('svg')
      .attr('width', '100%')
      .attr('height', '100%');

    //define group
    var group = canvas.append('g')
      .attr('transform', 'translate(100, 100)')

//create component in group
    //create circles
    group.selectAll('circle')
      .data(circles)
      .enter()
      .append('circle')
      .attr('cx', function(d, i){
        return d + 100 * i;
      })
      .attr('cy', function(d, i){
        return d + 100 * i;
      })
      .attr('r', function(d, i){
        return d;
      })
      .attr('fill', 'white')
      .attr('stroke', 'black')
      .attr('stroke-width', 1)
      .call(d3.drag().on('drag', function(d){
          d3.select(this)
            .attr("cx", d3.event.x)
            .attr("cy", d3.event.y);
        }))
      //define lines
      var line = d3.line()
        .x(function(d){return d.x})
        .y(function(d){return d.y})
      group.selectAll('path')
        .data([lines])
        .enter()
        .append('path')
        .attr('d', line)
        .attr('fill', 'none')
        .attr('stroke', 'black')
        .attr('stroke-width', 5)
        .call(d3.drag().on('drag', function(d){
            console.log(d3.event.x)
            d3.select(this)
            .attr("transform", "translate(" + d3.event.x + ", " + d3.event.y + ")");
          }))
  }



  render() {
    return (
      <Row>
        <Col span={4}>
          <div style={{height:'71em', borderRight:'solid'}}>

          </div>
        </Col>
        <Col span={16}>
          <div id="canvas" style={{height:'71em'}}>

          </div>
        </Col>
        <Col span={4}>
          <div style={{height:'71em', borderLeft:'solid'}}>

          </div>
        </Col>
      </Row>
    )
  }
}
const ScenesContainerForm = Form.create()(ScenesContainer);
ScenesContainer.contextTypes = {
  router: React.PropTypes.object
}
export default ScenesContainerForm
