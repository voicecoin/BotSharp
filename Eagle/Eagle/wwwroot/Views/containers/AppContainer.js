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
  chat = () => {
    window.__lc = window.__lc || {};
    window.__lc.license = 8946384;
    (function() {
      var lc = document.createElement('script'); lc.type = 'text/javascript'; lc.async = true;
      lc.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'cdn.livechatinc.com/tracking.js';
      var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(lc, s);
    })();
  }
  componentDidMount =() => {
    this.chat();
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
