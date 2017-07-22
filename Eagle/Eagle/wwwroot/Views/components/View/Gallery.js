import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox, Button, Card} from 'antd';
const FormItem = Form.Item;
import {Carousel} from 'react-bootstrap';
export class Gallery extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      data : this.props.data
    }
  }

  render() {
    const data = this.state.data[0].data.map((values) =>
    <Carousel.Item key={values.id}>
    <Card title={values.userName} extra={<a href="#">More</a>} style={{ width: 900, height:500, marginLeft:'20%' }}>
      <p>{values.email}</p>
    </Card>
    </Carousel.Item>
    )
    return (
      <div style={{marginTop:'10%', textAlign:'center'}}>
        <Carousel interval={null}>
          {data}
        </Carousel>
      </div>
    )
  }
}

const GalleryForm = Form.create()(Gallery);
Gallery.contextTypes = {
  router : React.PropTypes.object
}
export default GalleryForm
