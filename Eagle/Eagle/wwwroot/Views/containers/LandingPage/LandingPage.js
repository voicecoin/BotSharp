import React from 'react'
import {Link} from 'react-router'
import { Form, Input, Tooltip, Icon, Cascader, Select, Row, Col, Checkbox,Button } from 'antd';

const landingLoginHeaderWel = require('../../Sources/images/Welcome-wang2.png')
export default class LandingContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
    }
  }


  render() {
    const landingStyle = {
      position: 'relative',
      marginTop:'25%',
      marginLeft:'40%'
    };
    return (
      <div className='landingLoginBox'>
          <div className='landingLogin'></div>
          <div className='landingLoginTop'>
            <div className='landingLoginHeader'>
              
              
            </div>
            <div className="landingLoginImg">
              <h3>YAYA</h3>
              <h2>Launch chatbot in five minutes.</h2>              
              <div className='LoginButtonBox'>
              <i className='btn_login_icon'></i>
                <Link to='/Login'>
                    <button className="btn_login">Log In</button>
                </Link>              
              </div>

            </div>
          </div>
      </div>

    )
  }
}
