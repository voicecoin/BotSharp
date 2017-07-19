import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox,Button } from 'antd';
const landingLoginHeaderLog = require('../../Sources/images/o.png');
const landingLoginHeaderWel = require('../../Sources/images/Welcome-wang2.png')
export default class LandingContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
    }
  }


  render() {
    return (
      <div style={{textAlign:'center', marginTop:'25%'}}>
        <Link to='/Login'>
            <Button type='primary'>Log In</Button>
        </Link>
      </div>
    )
  }
}
