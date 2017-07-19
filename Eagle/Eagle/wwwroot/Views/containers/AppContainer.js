import React, {Component} from 'react'
import MyMenu from '../components/MyMenu';
import TopNav from '../components/TopNav';
import BackButton from '../components/GoBack'
import {Button} from 'antd';
import FreeScrollBar from 'react-free-scrollbar';
export default class AppContainer extends Component {
  constructor(){
    super();
  }
  render() {
    return (
      <div className='allenbox'>
        <MyMenu></MyMenu>
        <div className='rightWrap'>
          <TopNav />
          <FreeScrollBar autohide={true} style={{height:'95%'}}>
          {this.props.children}

          </FreeScrollBar>
        </div>
      </div>
    )
  }
}
